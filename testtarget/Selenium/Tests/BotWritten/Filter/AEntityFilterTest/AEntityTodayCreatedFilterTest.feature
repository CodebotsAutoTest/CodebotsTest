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

@BotWritten @Filter
Feature: AEntity filtered by today's created Feature

@AEntity
Scenario: AEntity filtered by today's created
Given I login to the site as a user
Given I have 10 valid AEntity entities
And I navigate to the AEntity backend page
When I click the filter Button on the AEntity page
Then The filter panel shows up with correct information
When I enter the created date filter starting from 1 days ago
And I apply the current filter
Then Each row has been created within the last 1 days