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

$serverReady = 0;
$clientReady = 0;

Write-Host "Building docker containers... This may take a couple of minutes.";

try {
	docker-compose --project-name "lm2348" up -d;

	Write-Host "Docker containers are up and running";
	Write-Host "Installing, running migrations and starting application";

	do {
		Start-Sleep 1;
		$serverStatus = docker inspect --format='{{.State.Health.Status}}' lm2348_server_1

		if ($serverStatus -eq "unhealthy") {
			Write-Host "Server failed to start.";
			exit 1;
		} elseif ($ServerStatus -eq "healthy" -and $serverReady -eq 0) {
			Write-Host "Server is ready";
			$serverReady=1;
			Write-Host "Waiting on client...";
		}

		$clientStatus = docker inspect --format='{{.State.Health.Status}}' lm2348_client_1;

		if ($clientStatus -eq "unhealthy") {
			Write-Host "Client failed to start.";
			exit 1;
		} elseif($clientStatus -eq "healthy" -and $clientReady -eq 0) {
			Write-Host "Client is ready;";
			$clientReady=1;
			Write-Host "Waiting on server...";
		}
	} Until((docker-compose --project-name "lm2348" ps | Select-String 'Up \(healthy\)' | Measure-Object -Line).Lines -gt 1)

	Write-Host "=================================";
	Write-Host "Application started successfully!";
	Write-Host "---------------------------------";
	Write-Host "Access your application at http://localhost:8000";
	Write-Host "=================================";
} catch {
	Write-Host "Exception Message: $($_.Exception.Message)" -ForegroundColor Red
}
finally {
	Read-Host "Press enter to close window..."
}