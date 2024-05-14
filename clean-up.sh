#!/bin/bash

echo "Deleting all applications k8s resources"
kubectl delete --recursive --force=true -f k8s 

echo "Deleting ingress nginx"
kubectl delete --force=true -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.8.2/deploy/static/provider/cloud/deploy.yaml