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
import { ChangeEventHandler } from 'react';
import * as uuid from 'uuid';
import classNames from 'classnames';
import InputWrapper from '../Inputs/InputWrapper';
import InputsHelper from '../Helpers/InputsHelper';
import { DisplayType } from '../Models/Enums';
import { action, computed, observable } from 'mobx';
import { observer } from 'mobx-react';
import { Button, Display } from 'Views/Components/Button/Button';
import { FileUploadPreview } from 'Views/Components/FileUpload/UploadPreview';
import If from 'Views/Components/If/If';

export interface FileUploadProps<T> {
	/**
	 * The model to load the result data into.
	 */
	model: T;
	/**
	 * The property to load the file into. The datatype of this field is the Javascript File type.
	 */
	modelProperty: string;
	/**
	 * Should a file preview be shown or a function to override the preview. If this is not set then the preview will
	 * not be displayed
	 */
	preview?: boolean | ((file?: File, onDelete?: () => void) => React.ReactNode);
	/**
	 * The content types to accept. This takes the format of the accept tag in a HTML input.
	 *
	 * See https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/file#accept
	 */
	contentType?: string;
	/**
	 * Allow only images? This will restrict the content type to images only (this can be overwritten by the contentType
	 * prop). This will also enable an image preview window to see the image to be uploaded before submitting.
	 */
	imageUpload?: boolean;
	/**
	 * Id for the component.
	 */
	id?: string;
	/**
	 * The name of the input.
	 */
	name?: string;
	/**
	 * The class name of the component.
	 */
	className?: string;
	/**
	 * The display mode for the input.
	 */
	displayType?: DisplayType;
	/**
	 * The label for the input.
	 */
	label?: string;
	/**
	 * Should the label be visible. If this is set the false the label will still exist in the aria-label attribute.
	 */
	labelVisible?: boolean;
	/**
	 * Is this field required.
	 */
	isRequired?: boolean;
	/**
	 * Is this field disabled.
	 */
	isDisabled?: boolean;
	/**
	 * Is this a readonly field. This will disable the field and remove all inputs.
	 */
	isReadOnly?: boolean;
	/**
	 * Is this a static input.
	 */
	staticInput?: boolean;
	/**
	 * The a tooltip for this input.
	 */
	tooltip?: string;
	/**
	 * A description for this input.
	 */
	subDescription?: string;
	/**
	 * Props to directly pass to the HTML input element.
	 */
	inputProps?: React.InputHTMLAttributes<Element>;
	/**
	 * Errors to display for this component.
	 */
	errors?: string | string[];
	/**
	 * An override for the onchange function.
	 * @param file The file the user selected
	 */
	onChange?: (file: File) => boolean;
	/**
	 * Callback after onChange has been completed. This is not called if onChange was overwritten.
	 * @param file The file the user selected
	 */
	onAfterChange?: (file: File) => void;
	/**
	 * Override for when the delete file button is pressed.
	 */
	onDelete?: () => void;
	/**
	 * Callback after a file has been cleared. This is not called if onDelete was overwritten.
	 */
	onAfterDelete?: () => void;
}

/**
 * This component provides an interface to load a file from the users device.
 */
@observer
export default class FileUpload<T> extends React.Component<FileUploadProps<T>> {
	protected uuid = uuid.v4();
	protected inputRef: HTMLInputElement | null = null;

	@observable
	public isBeingHovered = false;

	@observable
	protected internalErrors: string[] = [];

	@computed
	public get file() {
		return this.props.model[this.props.modelProperty] as File;
	};

	@computed
	public get disableDelete() {
		return this.props.isRequired || this.props.isDisabled || this.props.isReadOnly;
	}

	@computed
	protected get acceptType() {
		const { contentType, imageUpload } = this.props;
		return contentType ?? (imageUpload ? 'image/*' : undefined);
	}

	@computed
	protected get errors() {
		const errorsProp = this.props.errors;
		if (typeof errorsProp === 'string') {
			return [...this.internalErrors, errorsProp];
		} else if (Array.isArray(errorsProp)) {
			return [...this.internalErrors, ...errorsProp];
		}
		return this.internalErrors;
	}

	@action
	public setFile = (file: File) => {
		if (this.props.onChange) {
			return this.props.onChange(file);
		}

		this.internalErrors = [];
		if (!this.validateContentType(file)) {
			const message =  `Content type ${file.type} is not valid for ${this.acceptType}`;
			this.internalErrors.push(message);
			console.warn(message);
			return false;
		}

		(this.props.model[this.props.modelProperty] as File) = file;
		this.props.onAfterChange?.(file);

		return true;
	};

	@action
	public clearFile = () => {
		this.internalErrors = [];
		if (this.props.onDelete) {
			return this.props.onDelete();
		}
		if (this.inputRef) {
			this.inputRef.value = '';
		}

		this.props.model[this.props.modelProperty] = undefined;
		this.props.onAfterDelete?.()
	};

	public validateContentType = (file: File) => {
		const types = this.acceptType?.split(',').map(x => x.trim());

		// If this is null then there is no validation
		if (!types) {
			return true;
		}

		// Iterate over each allowed type and validate it against the file
		for (const type of types) {
			// Check file content types
			if (file.type === type) {
				return true;
			}

			// File extension match
			if (type.startsWith('.') && file.name.endsWith(type)) {
				return true;
			}

			// Check special content types
			if (type === 'audio/*' || type === 'video/*' || type === 'image/*') {
				const specialType = type.replace('/*', '');
				if (file.type.startsWith(specialType)) {
					return true;
				}
			}
		}

		return false;
	};

	protected onChange: ChangeEventHandler<HTMLInputElement> = event => {
		const { files } = event.target;
		if (files) {
			for (let i = 0; i < files.length; i++) {
				this.setFile(files[i]);
			}
		}
	};

	protected onDragOver = (event: React.DragEvent) => event.preventDefault();

	@action
	protected onDragEnter = () => this.isBeingHovered = true;

	@action
	protected onDragLeave = () => this.isBeingHovered = false;

	@action
	protected onDrop = (event: React.DragEvent) => {
		event.preventDefault();
		this.isBeingHovered = false;
		const file = event.dataTransfer.files[0];
		if (file) {
			this.setFile(file);
		}
	};

	protected onClick = () => {
		this.inputRef?.focus();
		this.inputRef?.click();
	};

	protected preview = () => {
		if (typeof this.props.preview === 'function') {
			return this.props.preview(this.file, this.disableDelete ? undefined : this.clearFile);
		}

		if (this.props.preview && this.file) {
			return <FileUploadPreview
				fileName={this.file.name}
				imagePreview={this.props.imageUpload}
				fileBlob={this.file}
				onDelete={this.disableDelete ? undefined : this.clearFile} />
		}
		return null;
	};

	public render() {
		const {
			name,
			className,
			displayType,
			label,
			isRequired,
			isDisabled,
			isReadOnly,
			staticInput,
			tooltip,
			subDescription,
		} = this.props;

		const wrapperId = this.uuid.toString();
		const fieldId = `${wrapperId}-field`;

		const labelVisible = (this.props.labelVisible === undefined) ? true : this.props.labelVisible;
		const ariaLabel = !labelVisible ? label : undefined;
		const ariaDescribedby = InputsHelper.getAriaDescribedBy(wrapperId, tooltip, subDescription);

		return (
			<div
				className={classNames(
					'upload',
					'upload__file',
					isReadOnly ? 'readonly' : undefined,
					className)}
				id={this.props.id}>
				<InputWrapper
					id={wrapperId}
					inputId={fieldId}
					className="file-input"
					displayType={displayType}
					isRequired={isRequired}
					staticInput={staticInput}
					tooltip={tooltip}
					subDescription={subDescription}
					label={label}
					labelVisible={labelVisible}
					errors={this.errors}>
					<input
						ref={instance => this.inputRef = instance}
						style={{display: 'none'}}
						aria-hidden="true"
						type="file"
						name={name}
						accept={this.acceptType}
						multiple={false}
						onChange={this.onChange}
						disabled={isDisabled}
						readOnly={staticInput}
						aria-label={ariaLabel}
						aria-describedby={ariaDescribedby}
						{...this.props.inputProps}/>
					<If condition={isReadOnly !== true}>
						<Button
							icon={{iconPos: 'icon-left', 'icon': 'upload'}}
							display={Display.Solid}
							disabled={isDisabled}
							onClick={this.onClick}>
							Choose File
						</Button>
						<div
							className={classNames(
								'upload__drag-area',
								this.isBeingHovered ? 'active' : undefined,
								isDisabled ? 'disabled' : undefined)}
							onDragOver={this.onDragOver}
							onDragEnter={this.onDragEnter}
							onDragLeave={this.onDragLeave}
							onDrop={this.onDrop}>
						</div>
					</If>
				</InputWrapper>
				<div className="file-preview">
					{this.preview()}
				</div>
			</div>
		);
	}
}
