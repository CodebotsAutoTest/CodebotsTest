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

Write-Host "Stopping the application...";

try {
	docker-compose --project-name "lm2348" down;

	do {
		Start-Sleep 1;
	} Until((docker-compose --project-name "lm2348" ps | Measure-Object -Line).Lines -lt 3)

	Write-Host "=================================";
	Write-Host "Application stopped successfully!";
	Write-Host "=================================";
} catch {
	Write-Host "Exception Message: $($_.Exception.Message)" -ForegroundColor Red
} finally {
	Read-Host "Press enter to close window..."
}