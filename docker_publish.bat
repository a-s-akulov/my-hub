echo off;
docker build -t andreyakulov/my-hub:api -f TicketsGenerator\TicketsGenerator\TicketsGeneratorServices.Api\Dockerfile .
docker push andreyakulov/my-hub:api

docker build -t andreyakulov/my-hub:web -f asd/TicketsGenerator/TicketsGenerator.Web\Dockerfile .
docker push andreyakulov/my-hub:web

@REM docker build -t andreyakulov/my-hub:aspire -f asd/TicketsGenerator/TicketsGenerator.AppHost\Dockerfile .
@REM docker push andreyakulov/my-hub:aspire


pause;