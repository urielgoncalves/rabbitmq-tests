# RabbitMQ tests

https://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html


## Run RabbitMQ container mapping host ports
docker run -d -p 15672:15672 -p 5672:5672 --name myrabbitmqc rabbitmq:3.8.14-management-alpine