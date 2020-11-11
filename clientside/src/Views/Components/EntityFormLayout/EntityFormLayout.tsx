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
import { observer } from 'mobx-react';
import { Model, IAttributeGroup } from 'Models/Model';
import { AttributeCRUDOptions } from 'Models/CRUDOptions';
import { getAttributeComponent } from '../CRUD/Attributes/AttributeFactory';
import _ from 'lodash';
import * as AttrUtils from "../../../Util/AttributeUtils";
import {AttributeFormMode, EntityFormMode} from '../Helpers/Common';
import { isRequired } from 'Util/EntityUtils';
import { FieldSet } from '../FieldSet/FieldSet';
import {IEntityAttributeBehaviour} from "../CRUD/EntityAttributeList";

interface IEntityFormLayout<T extends Model> {
	/** The model containing the data to render */
	model: T;
	/** If the created and modified fields should be displayed as the default fields */
	displayCreatedModifed?: boolean;
	/** The callback function used to get error messges by attribute name from outside the component */
	getErrorsForAttribute?: (attributeName: string) => string[];
	/** The current form mode. EntityFormMode: VIEW = 'view', CREATE = 'create', EDIT = 'edit' */
	formMode?: EntityFormMode;
	/** The callback function which will be triggered when any of the attribute got changed, no matter the onBlur (out focus) event is triggered or not */
	onAttributeAfterChange?: (attributeName: string) => void;
	/** The callback function which will be triggered when any of the attribute got changed, and also onBlur (out focus) event is triggered */
	onAttributeChangeAndBlur?: (attributeName: string) => void;
	/** Specifies if an attribute should be read-only, editable or hidden */
	attributeBehaviours?: Array<IEntityAttributeBehaviour>;
}

@observer
export class EntityFormLayout<T extends Model> extends React.Component<IEntityFormLayout<T>> {
	private getOneFieldSet(attrGroup: IAttributeGroup, attrs: AttributeCRUDOptions[]) {
		// % protected region % [Modify getOneFieldSet method here] off begin
		const id = attrGroup.id.toString();
		return (
			<FieldSet
				id={id}
				name={attrGroup.name}
				className={_.camelCase(attrGroup.name)}
				showName={attrGroup.showName ? attrGroup.showName : true}
				key={id}>
				{
					attrs
					.sort((a, b) => {
						if(b.order === undefined){
							return -1;
						} else if (a.order === undefined) {
							return 1;
						} else {
							return a.order - b.order;
						}
					})
					.map(attributeOption =>{
						const formMode = this.getAttributeViewMode(attributeOption);

						if (!formMode) {
							return null;
						}

						return getAttributeComponent(
							attributeOption,
							this.props.model,
							this.props.getErrorsForAttribute ? this.props.getErrorsForAttribute(attributeOption.attributeName) : [],
							formMode,
							isRequired(this.props.model, attributeOption.attributeName),
							this.props.onAttributeAfterChange,
							this.props.onAttributeChangeAndBlur
						)
					})
				}
			</FieldSet>
		);
		// % protected region % [Modify getOneFieldSet method here] end
	}

	private getAttributeViewMode = (attributeOption: AttributeCRUDOptions) => {
		let viewMode = this.props.formMode;
		if (this.props.attributeBehaviours) {
			const attributeBehaviour = this.props.attributeBehaviours
				.find(x => x.name === attributeOption.attributeName);
			if (attributeBehaviour) {
				switch(attributeBehaviour.behaviour) {
					case AttributeFormMode.EDIT:
						viewMode = EntityFormMode.EDIT;
						break;
					case AttributeFormMode.VIEW:
						viewMode = EntityFormMode.VIEW;
						break;
					case AttributeFormMode.HIDE:
					default:
						return null;
				}
			}
		}
		return viewMode;
	};

	render() {
		let attributeOptions = this.props.model.getAttributeCRUDOptions();
		let defualtDateAttrs: AttributeCRUDOptions[] = [];
		if (this.props.displayCreatedModifed) {
			let createDateAttr = new AttributeCRUDOptions('created', {name:'Created', displayType: 'datepicker', headerColumn: false, searchable: true, searchFunction: 'equal', searchTransform: AttrUtils.standardiseDate});
			createDateAttr.isReadonly = true;
			let modifiedDateAttr = new AttributeCRUDOptions('modified', {name:'Modified', displayType: 'datepicker', headerColumn: false, searchable: true, searchFunction: 'equal', searchTransform: AttrUtils.standardiseDate});
			modifiedDateAttr.isReadonly = true;
			defualtDateAttrs = [createDateAttr, modifiedDateAttr];
		}

		const model = this.props.model;
		/** If the attributeGroups is not defined or empty in the model class, the fields in the form should be shown as default order and with no grouping.
		 * Otherwise display them with grouping and ordering defined in the model class
		 */
		// % protected region % [Modify how attributes are rendered to the page here] off begin
		if (model.attributeGroups && model.attributeGroups.length > 0) {
			return (
				<>
					{
						model.attributeGroups
							.sort((a, b) => { return a.order - b.order })
							.map(attributeGroup =>
								this.getOneFieldSet(attributeGroup,
									attributeOptions.filter(attr => attr.groupId === attributeGroup.id)
								)
							)
					}
					{
						defualtDateAttrs
							.map(attributeOption =>
								getAttributeComponent(
									attributeOption,
									model,
									this.props.getErrorsForAttribute ? this.props.getErrorsForAttribute(attributeOption.attributeName) : [],
									this.props.formMode,
									isRequired(model, attributeOption.attributeName),
									this.props.onAttributeAfterChange,
									this.props.onAttributeChangeAndBlur
								)
							)
					}
				</>
			);
		} else {
			attributeOptions = [...attributeOptions, ...defualtDateAttrs];
			return attributeOptions
				.sort((a, b) => {
					if(b.order === undefined){
						return -1;
					} else if (a.order === undefined) {
						return 1;
					} else {
						return a.order - b.order;
					}
				})
				.map(attributeOption => {
					const formMode = this.getAttributeViewMode(attributeOption);

					if (!formMode) {
						return null;
					}

					return getAttributeComponent(
						attributeOption,
						model,
						this.props.getErrorsForAttribute ? this.props.getErrorsForAttribute(attributeOption.attributeName) : [],
						formMode,
						isRequired(model, attributeOption.attributeName),
						this.props.onAttributeAfterChange,
						this.props.onAttributeChangeAndBlur
					)
				});
		}
		// % protected region % [Modify how attributes are rendered to the page here] end
	}
}