import React, { Component } from 'react';

class Form extends Component {
	constructor() {
		super();

		this.state = {
			addItem : "",
		}
		this.handleCancel = this.handleCancel.bind(this);
		this.handleSubmit = this.handleSubmit.bind(this);
		this.handleChange = this.handleChange.bind(this);
	}

	// Cancel
	handleCancel() {
		this.props.onClickCancel();
	}

	// Change
	handleChange(event) {
		this.setState({
			addItem : event.target.value
		});
	}

	// Submit
	handleSubmit() {
		this.props.onlickSubmit(this.state.addItem);
	}
	
	// Display
  	render() {
    	return (
    		<>
			<div className="row justify-content-end mb-4">
				<div className="row justify-content-between col-6 m-0">
					<input value={this.state.addItem} onChange={this.handleChange} type="Text" className="form-control col-5" placeholder="Task name..." />
					<select type="Text" className="form-control col-2">
  						<option value="0">Small</option>
  						<option value="1">Medium</option>
  						<option value="2">High</option>
					</select>
					<button onClick={this.handleSubmit} className="btn btn-primary col-2">Submit</button>
					<button onClick={this.handleCancel} className="btn btn-warning col-2">Cancel</button>
				</div>
			</div>
      		</>
    	);
  	}
}

export default Form;