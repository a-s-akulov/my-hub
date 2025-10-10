echo off;
docker build -t andreyakulov/my-hub:web -f asd/TicketsGenerator/TicketsGenerator.Web\Dockerfile .
docker push andreyakulov/my-hub:web


pause;