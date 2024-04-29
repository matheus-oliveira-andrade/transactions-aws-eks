## Introdution

This project aims to provide an API for retrieving account movements, applying concepts like async comunications, observability and use of container orchestration. It is built upon the foundation of the [existing repository here](https://github.com/matheus-oliveira-andrade/transactions) and is designed to run seamlessly in a Kubernetes environment.

## Getting Started

Project to expose, through an API, the report of movements from the accounts. Transactions are created by the `Seed` console application and published to a topic in `RabbitMQ`, then read by the `Movements.AsyncReceiver` application, which processes and saves the data in the `PostgreSQL` database. This data is then exposed through the `Movements.Api` at the `/report/{{accountId}}` endpoint.

### How to run on Kubernetes using docker-desktop

1 - execute script [`run.sh`](run.sh) to create all required resources
   ```bash
   # use parameter --build-push to build and push docker images
   ./run.sh # --build-push
   ```

2 - Access movements public API 
   - [Swagger API docs movements](http://localhost/movements/swagger)
   - [Example get movements report for account 123456-78](http://localhost/movements/v1/report/123456-78)

3 - Logs in kibana
   - [Local Kibana address](http://localhost/kibana)
   - Configuring index pattern for see logs:
       1. [Access](http://localhost/kibana/app/management/kibana/indexPatterns)
       2. Create data view
       3. Name: fluentd-logs
       4. Create data view button

4 - Clean up, run script [`clean-up.sh`](clean-up.sh)
   ```bash
   ./clean-up.sh
   ```
### Technologies

- `C#` was used as the language with `.net 6`, following some of the concepts of `clean architecture`. For `unit tests`, `xunit` and `moq` were used.
- `Docker` was used for the application containers with `kubernetes` for container orchestration.
- `PostgreSQL` was chosen as the database.
- `RabbitMQ` was chosen as the message broker.
- `Fluentd` was used for log aggregation, sending the logs to `Elastic Search`.
- `Kibana` was used for log visualization.
- `GitHub Actions` were used for `CI` while the application was being developed, built, and tested on each push.

### Architecture

![image](https://github.com/matheus-oliveira-andrade/transactions/assets/32457879/1ab7e4cd-bb39-4ff9-bf0a-b5a9e4f57d06)

