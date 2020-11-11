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
import FileUpload from 'Views/Components/FileUpload/FileUpload';
import { Model } from 'Models/Model';
import { IAttributeProps } from 'Views/Components/CRUD/Attributes/IAttributeProps';
import { action, observable } from 'mobx';
import { observer } from 'mobx-react';
import { SERVER_URL } from 'Constants';
import { EntityFormMode } from 'Views/Components/Helpers/Common';
import { FileUploadPreview, UploadPreview } from 'Views/Components/FileUpload/UploadPreview';

export interface AttributeFileProps<T extends Model> extends IAttributeProps<T> {
	fileAttribute: string;
	imageOnly?: boolean;
}

interface FileMetadata {
	id: string;
	created: string;
	modified: string;
	fileName: string;
	contentType: string;
	length: number;
}

@observer
export default class AttributeFile<T extends Model> extends React.Component<AttributeFileProps<T>> {
	protected readonly initialFileId?: string;

	@observable
	protected fileMetadata?: FileMetadata;

	@action
	protected onFetchSucceeded = (metadata: FileMetadata) => {
		this.fileMetadata = metadata;
	};

	@action
	protected onAfterDelete = () => {
		this.props.model[this.props.options.attributeName] = undefined;
	};

	constructor(props: AttributeFileProps<T>) {
		super(props);

		if (this.props.model[this.props.options.attributeName]) {
			this.initialFileId = this.props.model[this.props.options.attributeName];
		}
	}

	protected loadFile = () => {
		const fileId = this.props.model[this.props.options.attributeName];
		if (fileId) {
			fetch(`${SERVER_URL}/api/files/metadata/${fileId}`)
				.then(x => x.json())
				.then(metadata => this.onFetchSucceeded(metadata));
		}
	};

	public componentDidMount() {
		// For view or edit mode load the initial file from the server
		switch (this.props.formMode) {
			case EntityFormMode.VIEW:
			case EntityFormMode.EDIT:
				this.loadFile();
		}
	}

	public render() {
		const {
			fileAttribute,
			isReadonly,
			imageOnly,
			model,
			isRequired,
			onAfterChange,
			className,
			errors,
			options
		} = this.props;

		return <FileUpload
			preview={(file, onDelete) => {
				if (!file && model[options.attributeName]) {
					return <UploadPreview
						download
						fileUrl={`${SERVER_URL}/api/files/${this.initialFileId}${imageOnly ? '' : '?download=true'}`}
						onDelete={onDelete}
						imagePreview={imageOnly}
						fileName={this.fileMetadata?.fileName}/>;
				}

				if (file) {
					return <FileUploadPreview
						fileBlob={file}
						onDelete={onDelete}
						imagePreview={imageOnly}
						fileName={file.name}/>;
				}

				return null;
			}}
			model={model}
			modelProperty={fileAttribute}
			imageUpload={imageOnly}
			label={options.displayName}
			errors={errors}
			className={className}
			isReadOnly={isReadonly}
			isRequired={isRequired}
			onAfterChange={onAfterChange}
			onAfterDelete={this.onAfterDelete}/>;
	}
}

@observer
export class FileListPreview extends React.Component<{ url: string }> {
	@observable
	private fileName?: string = undefined;
	
	@action
	private setFileName = (metadata: FileMetadata) => {
		this.fileName = metadata.fileName;
	};
	
	public componentDidMount() {
		fetch(`${SERVER_URL}/api/files/metadata/${this.props.url}`)
			.then(x => x.json())
			.then(this.setFileName);
	}
	
	public render() {
		return <a
			href={`${SERVER_URL}/api/files/${this.props.url}?download=true`}
			target="_blank"
			rel="noopener noreferrer"
			className="btn btn--icon icon-download icon-right">
			{this.fileName ?? 'Download'}
		</a>
	}
}