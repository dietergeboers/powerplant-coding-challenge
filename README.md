# how to build and run

After cloning to your local repo go to the directory EngieChallenge\EngieChallange 
in your shell enviroment and execute dotnet run.

## alternatives - in visual studio

Open the project in visual studio by doubleclicking EngieChallenge\EngieChallange.sln and 
press the start button.

## alternatives - docker

Navigate with your shell to  EngieChallenge and run docker build and docker run

* EngieChallange>docker build --rm -t yournamehere/name:latest .

* docker run --rm -p 8888:8888 -e ASPNETCORE_HTTP=http://+:8888 -e ASPNETCORE_URLS=http://+:8888 yournamehere/name:latest 