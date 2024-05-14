#!/bin/bash

echo "Deleting all applications k8s resources"
kubectl delete --recursive --force=true -f k8s