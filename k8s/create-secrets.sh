#!/bin/bash

kubectl create secret generic rabbit-mq-password --from-literal RabbitMq_Password='123Abc!@#'

kubectl create secret generic movements-db-password --from-literal MovementsDb_Password='123Abc!@#'
