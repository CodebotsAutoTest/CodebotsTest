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

@BotWritten @admin @nav
Feature: Admin Nav Section

Scenario: Navigate to admin landing page
	Given I login to the site as a user
	Then I assert that the admin bar is on the Admin
	When I click on the Topbar Link
	Then I assert that the admin bar is on the Frontend
	Then The Admin Nav Menu is displayed

Scenario: Navigate to home page
	Given I login to the site as a user
	Then I assert that the admin bar is on the Admin
	When I click on the Topbar Link
	Then I assert that the admin bar is on the Frontend
	When I click the home link of the admin nav section
	Then I assert that the admin bar is on the Frontend

Scenario: Verify the number of Admin Submenus
	Given I login to the site as a user
	Then I assert that the admin bar is on the Admin
	When I click on the Topbar Link
	Then I assert that the admin bar is on the Frontend
	When I click on Users Nav link on the Admin Nav section
	Then I assert that 1 Nav links are displayed
	When I click on Entities Nav link on the Admin Nav section
	Then I assert that 2 Nav links are displayed

Scenario: Verify the admin submenus
	Given I login to the site as a user
	Then I assert that the admin bar is on the Admin
	When I click on the Topbar Link
	Then I assert that the admin bar is on the Frontend
	When I click on Users Nav link on the Admin Nav section
	Then I see the Admin Submenus like
	| Users |
	| All Users |
	When I click on Entities Nav link on the Admin Nav section
	Then I see the Admin Submenus like
	| Entities |
	| a |
	| b |
