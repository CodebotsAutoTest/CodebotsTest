/*
 * @bot-written
 * 
 * WARNING AND NOTICE
 * Any access, download, storage, and/or use of this source code is subject to the terms and conditions of the
 * Full Software Licence as accepted by you before being granted access to this source code and other materials,
 * the terms of which can be accessed on the Codebots website at https://codebots.com/full-software-licence. Any
 * commercial use in contravention of the terms of the Full Software Licence may be pursued by Codebots through
 * licence termination and further legal action, and be required to indemnify Codebots for any loss or damage,
 * including interest and costs. You are deemed to have accepted the terms of the Full Software Licence on any
 * access, download, storage, and/or use of this source code.
 * 
 * BOT WARNING
 * This file is bot-written.
 * Any changes out side of "protected regions" will be lost next time the bot makes any changes.
 */
import * as React from 'react';
import { AttributeCRUDOptions } from 'Models/CRUDOptions';
import { Model } from 'Models/Model';
import AttributeTextField from './AttributeTextField';
import AttributeTextArea from './AttributeTextArea';
import AttributeReferenceCombobox from './AttributeReferenceCombobox';
import AttributeDatePicker from './AttributeDatePicker';
import AttributeTimePicker from './AttributeTimePicker';
import AttributeCheckbox from './AttributeCheckbox';
import AttributePassword from './AttributePassword';
import AttributeDisplayField from './AttributeDisplayField';
import AttributeReferenceMultiCombobox from './AttributeReferenceMultiCombobox';
import AttributeDateTimePicker from './AttributeDateTimePicker';
import AttributeEnumCombobox from './AttributeEnumCombobox';
import { EntityFormMode } from 'Views/Components/Helpers/Common';
import AttributeFile from 'Views/Components/CRUD/Attributes/AttributeFile';
// % protected region % [Add extra imports here] off begin
// % protected region % [Add extra imports here] end

export function getAttributeComponent (
	attributeOptions: AttributeCRUDOptions,
	model: Model,
	errors: string[],
	formMode: EntityFormMode = EntityFormMode.VIEW,
	isRequired: boolean = false,
	onAfterChange?: (attributeName: string) => void,
	onChangeAndBlur?: (attributeName: string) => void)
{
	const className = attributeOptions.className 
		? `${attributeOptions.attributeName} ${attributeOptions.className}`
		: attributeOptions.attributeName;

	const isReadonly = formMode === EntityFormMode.VIEW || attributeOptions.isReadonly;

	const displayType = {
		[EntityFormMode.VIEW]: attributeOptions.readFieldType,
		[EntityFormMode.CREATE]: attributeOptions.createFieldType,
		[EntityFormMode.EDIT]: attributeOptions.updateFieldType,
	}[formMode];

	switch (displayType) {
		case 'textfield':
			// % protected region % [Override textfield here] off begin
			return <AttributeTextField
				key={attributeOptions.attributeName}
				model={model}
				options={attributeOptions}
				errors={errors}
				className={className}
				isReadonly={isReadonly}
				isRequired={isRequired}
				formMode={formMode}
				onAfterChange={() => {
					if (!!onAfterChange) {
						onAfterChange(attributeOptions.attributeName);
					}
					if (attributeOptions.onAfterChange) {
						attributeOptions.onAfterChange(model);
					}
				}}
				onChangeAndBlur={() => {
					if (!!onChangeAndBlur) {
						onChangeAndBlur(attributeOptions.attributeName);
					}
				}}
				{...attributeOptions.inputProps}
				/>;
			// % protected region % [Override textfield here] end
		case 'textarea':
			// % protected region % [Override textarea here] off begin
			return <AttributeTextArea
				key={attributeOptions.attributeName}
				model={model}
				options={attributeOptions}
				errors={errors}
				className={className}
				isReadonly={isReadonly}
				isRequired={isRequired}
				formMode={formMode}
				onAfterChange={() => {
					if (!!onAfterChange) {
						onAfterChange(attributeOptions.attributeName);
					}
					if (attributeOptions.onAfterChange) {
						attributeOptions.onAfterChange(model);
					}
				}}
				onChangeAndBlur={() => {
					if (!!onChangeAndBlur) {
						onChangeAndBlur(attributeOptions.attributeName);
					}
				}}
				{...attributeOptions.inputProps}
				/>;
			// % protected region % [Override textarea here] end
		case 'password':
			// % protected region % [Override password here] off begin
			return <AttributePassword
				key={attributeOptions.attributeName}
				model={model}
				options={attributeOptions}
				errors={errors}
				className={className}
				isReadonly={isReadonly}
				isRequired={isRequired}
				formMode={formMode}
				onAfterChange={() => {
					if (!!onChangeAndBlur) {
						onChangeAndBlur(attributeOptions.attributeName);
					}
					if (attributeOptions.onAfterChange) {
						attributeOptions.onAfterChange(model);
					}
				}}
				{...attributeOptions.inputProps}
				/>;
			// % protected region % [Override password here] end
		case 'datepicker':
			// % protected region % [Override date picker here] off begin
			return <AttributeDatePicker
				key={attributeOptions.attributeName}
				model={model}
				options={attributeOptions}
				className={className}
				isReadonly={isReadonly}
				isRequired={isRequired}
				formMode={formMode}
				onAfterChange={() => {
					if (!!onChangeAndBlur) {
						onChangeAndBlur(attributeOptions.attributeName);
					}
					if (attributeOptions.onAfterChange) {
						attributeOptions.onAfterChange(model);
					}
				}}
				{...attributeOptions.inputProps}
				/>;
			// % protected region % [Override date picker here] end
		case 'timepicker':
			// % protected region % [Override time picker here] off begin
			return <AttributeTimePicker
				key={attributeOptions.attributeName}
				model={model}
				options={attributeOptions}
				className={className}
				isReadonly={isReadonly}
				isRequired={isRequired}
				formMode={formMode}
				onAfterChange={() => {
					if (!!onChangeAndBlur) {
						onChangeAndBlur(attributeOptions.attributeName);
					}
					if (attributeOptions.onAfterChange) {
						attributeOptions.onAfterChange(model);
					}
				}}
				{...attributeOptions.inputProps}
				/>;
			// % protected region % [Override time picker here] end
		case 'datetimepicker':
			// % protected region % [Override datetime picker here] off begin
			return <AttributeDateTimePicker
				key={attributeOptions.attributeName}
				model={model}
				options={attributeOptions}
				className={className}
				isReadonly={isReadonly}
				isRequired={isRequired}
				formMode={formMode}
				onAfterChange={() => {
					if (!!onChangeAndBlur) {
						onChangeAndBlur(attributeOptions.attributeName);
					}
					if (attributeOptions.onAfterChange) {
						attributeOptions.onAfterChange(model);
					}
				}}
				{...attributeOptions.inputProps}
				/>;
			// % protected region % [Override datetime picker here] end
		case 'checkbox':
			// % protected region % [Override checkbox here] off begin
			return <AttributeCheckbox
				key={attributeOptions.attributeName}
				model={model}
				options={attributeOptions}
				className={className}
				isReadonly={isReadonly}
				isRequired={isRequired}
				formMode={formMode}
				onAfterChange={() => {
					if (!!onChangeAndBlur) {
						onChangeAndBlur(attributeOptions.attributeName);
					}
					if (attributeOptions.onAfterChange) {
						attributeOptions.onAfterChange(model);
					}
				}}
				{...attributeOptions.inputProps}
				/>;
			// % protected region % [Override checkbox here] end
		case 'displayfield':
			// % protected region % [Override displayfield here] off begin
			return <AttributeDisplayField
				key={attributeOptions.attributeName}
				model={model}
				options={attributeOptions}
				errors={errors}
				className={className}
				formMode={formMode}
				onAfterChange={() => {
					if (!!onChangeAndBlur) {
						onChangeAndBlur(attributeOptions.attributeName);
					}
					if (attributeOptions.onAfterChange) {
						attributeOptions.onAfterChange(model);
					}
				}}
				{...attributeOptions.inputProps}
				/>;
			// % protected region % [Override displayfield here] end
		case 'reference-combobox':
			// % protected region % [Override reference-combobox here] off begin
			if (attributeOptions.referenceTypeFunc === undefined) {
				throw new Error('Must have a defined referenceType for display type' + attributeOptions.displayType);
			}
			return <AttributeReferenceCombobox
				key={attributeOptions.attributeName}
				model={model}
				options={attributeOptions}
				referenceType={attributeOptions.referenceTypeFunc()}
				errors={errors}
				optionEqualFunc={attributeOptions.optionEqualFunc}
				className={className}
				isReadonly={isReadonly}
				isRequired={isRequired}
				fetchReferenceEntity={attributeOptions.isJoinEntity}
				formMode={formMode}
				onAfterChange={() => {
					if (!!onChangeAndBlur) {
						onChangeAndBlur(attributeOptions.attributeName);
					}
					if (attributeOptions.onAfterChange) {
						attributeOptions.onAfterChange(model);
					}
				}}
				{...attributeOptions.inputProps}
				/>;
			// % protected region % [Override reference-combobox here] end
		case 'reference-multicombobox':
			// % protected region % [Override refernce-multicombobox here] off begin
			if (attributeOptions.referenceTypeFunc === undefined) {
				throw new Error('Must have a defined referenceType for display type' + attributeOptions.displayType);
			}

			return <AttributeReferenceMultiCombobox
				key={attributeOptions.attributeName}
				model={model}
				options={attributeOptions}
				referenceType={attributeOptions.referenceTypeFunc()}
				referenceResolveFunction={attributeOptions.referenceResolveFunction}
				optionEqualFunc={attributeOptions.optionEqualFunc}
				errors={errors}
				isJoinEntity={attributeOptions.isJoinEntity}
				disableDefaultOptionRemoval={attributeOptions.disableDefaultOptionRemoval}
				className={className}
				isReadonly={isReadonly}
				isRequired={isRequired}
				formMode={formMode}
				onAfterChange={() => {
					if (attributeOptions.onAfterChange) {
						attributeOptions.onAfterChange(model);
					}
				}}
				{...attributeOptions.inputProps} />;
			// % protected region % [Override refernce-multicombobox here] end
		case 'enum-combobox':
			// % protected region % [Override enum-combobox here] off begin
			if (attributeOptions.enumResolveFunction === undefined) {
				throw new Error('Must have a defined enumType for display type' + attributeOptions.displayType);
			}
			return <AttributeEnumCombobox
				key={attributeOptions.attributeName}
				model={model}
				options={attributeOptions}
				errors={errors}
				className={className}
				isReadonly={isReadonly}
				isRequired={isRequired}
				formMode={formMode}
				onAfterChange={() => {
					if (!!onChangeAndBlur) {
						onChangeAndBlur(attributeOptions.attributeName);
					}
					if (attributeOptions.onAfterChange) {
						attributeOptions.onAfterChange(model);
					}
				}}
				/>;
			// % protected region % [Override enum-combobox here] end
		case 'file':
			// % protected region % [Override file input here] off begin
			if (attributeOptions.fileAttribute === undefined) {
				throw new Error(`Must have a defined file attribute for ${attributeOptions.attributeName}`)
			}
			return <AttributeFile
				key={attributeOptions.attributeName}
				model={model}
				options={attributeOptions}
				className={className}
				isReadonly={isReadonly}
				errors={errors}
				isRequired={isRequired}
				fileAttribute={attributeOptions.fileAttribute}
				formMode={formMode}
				onAfterChange={() => {
					if (attributeOptions.onAfterChange) {
						attributeOptions.onAfterChange(model);
					}
				}}
				{...attributeOptions.inputProps} />;
			// % protected region % [Override file input here] end
		case 'hidden':
			// % protected region % [Override hidden here] off begin
			return null;
			// % protected region % [Override hidden here] end
		// % protected region % [Add more customized cases here] off begin
		// % protected region % [Add more customized cases here] end
		default:
			throw new Error(`No attribute component is defined to handle ${attributeOptions.displayType} for attr ${attributeOptions.attributeName}`);
	}
}