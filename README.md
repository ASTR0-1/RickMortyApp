# Docker Setup

To build docker image you need to run:<br/>
<code>docker build -t rickmortapi -f ./RickMortyApp.Api/Dockerfile .</code>

<u>From the root folder</u> /RickMortyApp

And then you can run containers with:<br/>
<code>docker run -p 5000:80 -p 5001:443 --name 'container_name' rickmortapi</code>