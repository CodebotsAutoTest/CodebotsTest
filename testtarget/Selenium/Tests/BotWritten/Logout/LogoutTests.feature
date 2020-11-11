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

@logout @BotWritten
Feature: Logout via url

	# % protected region % [Customize Logout via url scenario here] off begin
	Scenario: Logout via url
	Given I login to the site as a user
		And I assert that the admin bar is on the Admin
	When I am logged out of the site
	Then I am redirected to the login page
	# % protected region % [Customize Logout via url scenario here] end

# % protected region % [Add any additional tests here] off begin
# % protected region % [Add any additional tests here] end