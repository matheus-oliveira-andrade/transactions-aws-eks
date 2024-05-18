data "local_file" "permissions" {
  filename = "./aws-load-balancer/permissions.json"
}

resource "aws_iam_policy" "aws_load_balancer_controller_policy" {
  name        = "aws_load_balancer_controller_policy"
  description = "Policy which will be used by role for service - for creating alb from within cluster by issuing declarative kube commands"
  policy      = data.local_file.permissions.content
}

resource "aws_iam_role" "aws_load_balancer_controller_role" {
  name = "${var.service_account_name}_role"

  assume_role_policy = <<POLICY
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Sid": "",
      "Effect": "Allow",
      "Principal": {
        "Federated": "${var.oidc_provider_arn}"     
},
      "Action": "sts:AssumeRoleWithWebIdentity",
      "Condition": {
        "StringEquals": {
          "${replace(var.oidc_provider, "https://", "")}:sub": "system:serviceaccount:${var.namespace}:${var.service_account_name}",
          "${replace(var.oidc_provider, "https://", "")}:aud": "sts.amazonaws.com"
        }
      }
    }
  ]
}
POLICY

  depends_on = [var.oidc_provider]
}

resource "aws_iam_role_policy_attachment" "aws_load_balancer_controller_rpa" {
  policy_arn = aws_iam_policy.aws_load_balancer_controller_policy.arn
  role       = aws_iam_role.aws_load_balancer_controller_role.name
  depends_on = [aws_iam_role.aws_load_balancer_controller_role]
}

resource "aws_iam_role_policy_attachment" "aws_load_balancer_controller_rpa_cni" {
  role       = aws_iam_role.aws_load_balancer_controller_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonEKS_CNI_Policy"
  depends_on = [aws_iam_role.aws_load_balancer_controller_role]
}

resource "kubernetes_service_account" "aws_load_balancer_controller-sa" {
  metadata {
    name        = var.service_account_name
    namespace   = var.namespace
    annotations = {
      "eks.amazonaws.com/role-arn" = aws_iam_role.aws_load_balancer_controller_role.arn
    }
    labels = {
      "app.kubernetes.io/name" = var.service_account_name
    }
  }
}

resource "kubernetes_cluster_role" "aws_load_balancer_controller_cr" {
  metadata {
    labels = {
      "app.kubernetes.io/name" = "${var.service_account_name}_role"
    }
    name = "${var.service_account_name}_role"
  }

  rule {
    api_groups = [""]
    resources  = [
      "configmaps",
      "endpoints",
      "events",
      "ingresses",
      "ingresses/status",
      "services",
      "pods/status",
    ]
    verbs = ["create", "get", "list", "update", "watch", "patch"]
  }

  rule {
    api_groups = [""]
    resources  = [
      "nodes",
      "pods",
      "secrets",
      "services",
      "namespaces",
    ]
    verbs = ["get", "list", "watch"]
  }
}

resource "kubernetes_cluster_role_binding" "aws_load_balancer_controller_rb" {
  metadata {
    labels = {
      "app.kubernetes.io/name" = "${var.service_account_name}_rb"
    }
    name = "${var.service_account_name}_rb"
  }

  role_ref {
    api_group = "rbac.authorization.k8s.io"
    kind      = "ClusterRole"
    name      = kubernetes_cluster_role.aws_load_balancer_controller_cr.metadata[0].name
  }

  subject {
    kind      = "ServiceAccount"
    name      = var.service_account_name
    namespace = var.namespace
  }
}

resource "helm_release" "aws_load_balancer_controller" {
  name       = "aws-load-balancer-controller"
  repository = "https://aws.github.io/eks-charts"
  chart      = "aws-load-balancer-controller"
  version    = "1.7.2"
  
  namespace  = var.namespace

  set {
    name  = "clusterName"
    value = var.cluster_name
  }

  set {
    name  = "serviceAccount.create"
    value = "false"
  }

  set {
    name  = "serviceAccount.name"
    value = var.service_account_name
  }
}