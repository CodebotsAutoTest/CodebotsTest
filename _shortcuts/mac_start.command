###
# @bot-written
# 
# WARNING AND NOTICE
# Any access, download, storage, and/or use of this source code is subject to the terms and conditions of the
# Full Software Licence as accepted by you before being granted access to this source code and other materials,
# the terms of which can be accessed on the Codebots website at https://codebots.com/full-software-licence. Any
# commercial use in contravention of the terms of the Full Software Licence may be pursued by Codebots through
# licence termination and further legal action, and be required to indemnify Codebots for any loss or damage,
# including interest and costs. You are deemed to have accepted the terms of the Full Software Licence on any
# access, download, storage, and/or use of this source code.
# 
# BOT WARNING
# This file is bot-written.
# Any changes out side of "protected regions" will be lost next time the bot makes any changes.
###
#!/bin/bash
cd -- "$(dirname "$0")"

serverReady=0;
clientReady=0;

set -e
echo "Building docker containers... This may take a couple of minutes.";
docker-compose --project-name "lm2348" up -d;

echo "Docker containers are up and running";
echo "Installing, running migrations and starting application";

until [ $(docker-compose --project-name "lm2348" ps | grep ' Up (healthy)' | wc -l) -gt 1 ]
do
	sleep 1;

	if [ "$(docker inspect --format='{{.State.Health.Status}}' lm2348_server_1)" = "unhealthy" ]; then
		echo "Server failed to start.";
		exit 1;
	elif [ "$(docker inspect --format='{{.State.Health.Status}}' lm2348_server_1)" = "healthy" ] && [ $serverReady -eq 0 ]; then
		echo "Server is ready";
		echo "Access available at http://localhost:8080";
		serverReady=1;
		echo "Waiting on client...";
	fi

	if [ "$(docker inspect --format='{{.State.Health.Status}}' lm2348_client_1)" = "unhealthy" ]; then
		echo "Client failed to start.";
		exit 1;
	elif [ "$(docker inspect --format='{{.State.Health.Status}}' lm2348_client_1)" = "healthy" ] && [ $clientReady -eq 0 ]; then
		echo "Client is ready;";
		echo "Access available at http://localhost:8000 or http://localhost:4200";
		clientReady=1;
		echo "Waiting on server...";
	fi
done

echo "=================================";
echo "Application started successfully!";
echo "---------------------------------";
echo "Access your application at http://localhost:8000";
echo "=================================";
