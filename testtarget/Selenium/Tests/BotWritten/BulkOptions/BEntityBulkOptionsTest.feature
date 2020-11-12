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
@BotWritten @bulkoptions @ignore
Feature: BEntity Bulk Option Selection
# WARNING: These Tests have been flagged as needing web fixes and are currently ignored

@BEntity
Scenario: Select all BEntitys on current page
Given I login to the site as a user
And I navigate to the BEntity backend page
When I select all entities on current page
Then I click the bulk bar cancel button
Then 0 entities on current page should be selected

@BEntity
Scenario: I attempt to select all BEntitys on all pages
Given I login to the site as a user
And I navigate to the BEntity backend page
When I select all entities on current page
Then The bulk options bar shows up with correct information
And I select all items in the collection
Then I click the bulk bar cancel button
Then 0 entities on current page should be selected