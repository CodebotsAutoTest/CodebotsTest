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
import classNames from 'classnames';
import { observer } from 'mobx-react';
import { action, computed, observable } from 'mobx';
import { Button, Display } from 'Views/Components/Button/Button';
import If from 'Views/Components/If/If';

export interface PreviewProps {
	/**
	 * The file to preview.
	 */
	fileName?: string;
	/**
	 * Should the preview show an image.
	 */
	fileUrl?: string;
	/**
	 * Callback to delete this file. If this function is not defined then this function will not be displayed.
	 */
	onDelete?: () => void;
	/**
	 * Is the preview of an image
	 */
	imagePreview?: boolean;
	/**
	 * Should name of this file be a link to the url
	 */
	download?: boolean;
}

/**
 * Preview display for the file upload component
 */
@observer
export class UploadPreview extends React.Component<PreviewProps> {
	@computed
	protected get className() {
		return classNames(this.props.imagePreview ? 'upload__image' : 'upload__file', 'preview');
	}

	public render() {
		return (
			<div className={this.className}>
				{this.props.imagePreview
					? <ImagePreview {...this.props} />
					: <FilePreview {...this.props} />}
			</div>
		);
	}
}

export interface FilePreviewProps extends Omit<PreviewProps, 'fileUrl'> {
	/**
	 * The file to accept
	 */
	fileBlob: Blob;
}

/**
 * Preview for the file upload that takes a base64 file instead of a URL
 */
@observer
export class FileUploadPreview extends React.Component<FilePreviewProps> {
	@observable
	protected base64File?: string = undefined;

	@action
	protected onImageLoaded = (event: ProgressEvent<FileReader>) => {
		const result = event.target?.result;
		if (typeof result === 'string') {
			this.base64File = result;
		}
	};

	protected loadFile = (file: Blob) => {
		const reader = new FileReader();
		reader.onload = this.onImageLoaded;
		reader.readAsDataURL(file);

	};

	public componentDidMount() {
		this.loadFile(this.props.fileBlob);
	}

	public componentDidUpdate(prevProps: Readonly<FilePreviewProps>) {
		if (this.props.fileBlob !== prevProps.fileBlob) {
			this.loadFile(this.props.fileBlob);
		}
	}

	public render() {
		if (!this.base64File) {
			return null;
		}

		const { fileName, onDelete, imagePreview } = this.props;

		return <UploadPreview
			fileName={fileName}
			onDelete={onDelete}
			imagePreview={imagePreview}
			fileUrl={this.base64File} />;
	}
}

const FileName = ({fileUrl, fileName, download}: PreviewProps) => {
	if (download && fileUrl) {
		return <a 
			className="file-name icon-download icon-right" 
			target="_blank"
			rel="noopener noreferrer"
			href={fileUrl}>
			{fileName}
		</a>;
	}
	return <p className="file-name">{fileName}</p>;
};

const ImagePreview = (props: PreviewProps) => (
	<div className="image">
		<img src={props.fileUrl} alt={props.fileName}/>
		<FileName {...props}/>
		<If condition={props.onDelete !== undefined}>
			<Button
				onClick={props.onDelete}
				display={Display.Outline}
				icon={{icon: 'bin-delete', iconPos: 'icon-left'}}>
			</Button>
		</If>
	</div>
);

const FilePreview = (props: PreviewProps) => (
	<div className="file">
		<FileName {...props}/>
		<If condition={props.onDelete !== undefined}>
			<Button
				onClick={props.onDelete}
				display={Display.Outline}
				icon={{icon:'bin-delete', iconPos:'icon-left'}}>
			</Button>
		</If>
	</div>
);