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
import { styleFn, Styles, StylesConfig } from 'react-select/lib/styles';

export function getComboboxStyles(existingStyles: StylesConfig): StylesConfig {
	const styles: Partial<Styles> = {...existingStyles};

	styles.option = getOptionStyles(existingStyles);
	styles.control = getControlStyles(existingStyles);
	styles.multiValue = getMultiValueStyles(existingStyles);
	styles.multiValueLabel = getMultiValueLabelStyles(existingStyles);
	styles.multiValueRemove = getMultiValueRemoveStyles(existingStyles);

	return styles;
}

function getOptionStyles(existingStyles: StylesConfig): styleFn {
	return (styles, data) => {
		const newStyles: React.CSSProperties = {
			...styles,
			cursor: 'pointer',
		};
		
		if (data.isSelected) {
			newStyles.backgroundColor = '#999999';
			newStyles.color = '#ffffff';
		}
		
		if (data.isFocused) {
			newStyles.backgroundColor = '#999999';
			newStyles.color = '#ffffff';
			newStyles[':active'].backgroundColor = '#999999'
		}
		
		if (existingStyles.option) {
			return existingStyles.option(newStyles, data);
		}
		return newStyles;
	}
}

function getControlStyles(existingStyles: StylesConfig): styleFn {
	return (styles, data) => {
		const newStyles = {
			...styles,
			border: 'solid 1px #222222',
			boxShadow: 'none',
			cursor: 'pointer',
			minWidth: '250px',
			borderRadius: '0',
		};
		
		newStyles[':hover'] = {
			border: 'solid 1px #222222',
		};
		
		if (existingStyles.control) {
			return existingStyles.control(newStyles, data);
		}
		return newStyles;
	}
}

function getMultiValueStyles(existingStyles: StylesConfig): styleFn {
	return (styles, data) => {
		const newStyles = {
			...styles,
			backgroundColor: '#222222',
			color: '#ffffff',
			padding: '0.4rem',
			fontSize: '.9rem',
			borderRadius: '0px'
		};
		
		if (data.data.isFixed) {
			newStyles.paddingRight = '9px';
		}
		
		if (existingStyles.multiValue) {
			return existingStyles.multiValue(newStyles, data);
		}
		return newStyles;
	}
}

function getMultiValueLabelStyles(existingStyles: StylesConfig): styleFn {
	return (styles, data) => {
		const newStyles = {
			...styles,
			color: '#ffffff',
		};
		
		if (existingStyles.multiValueLabel) {
			return existingStyles.multiValueLabel(newStyles, data);
		}
		return newStyles;
	}
}

function getMultiValueRemoveStyles(existingStyles: StylesConfig): styleFn {
	return (styles, data) => {
		const newStyles = {
			...styles,
			':hover': {
				color: '#E84D38',
			},
			paddingLeft: '1rem',
		};
		
		if (data.data.isFixed) {
			newStyles.display = 'none';
		}
		
		if (existingStyles.multiValueRemove) {
			return existingStyles.multiValueRemove(newStyles, data);
		}
		return newStyles;
	}
}