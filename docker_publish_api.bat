echo off;
docker build -t andreyakulov/my-hub:api -f TicketsGenerator\TicketsGenerator\TicketsGeneratorServices.Api\Dockerfile .
docker push andreyakulov/my-hub:api


pause;