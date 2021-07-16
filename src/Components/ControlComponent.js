import React, { Component } from 'react';

class ControlComponent extends Component {
	constructor(props) {
		super(props);

		this.state = {
			strSearch : "",
		}
		this.handleAdd = this.handleAdd.bind(this);
		this.handleSearch = this.handleSearch.bind(this);
		this.handleChange = this.handleChange.bind(this);
		this.handleClear = this.handleClear.bind(this);
	}

	// Add task
	handleAdd() {
		this.props.onClickAdd();
	}

	// Search
	handleSearch() {
		this.props.onClickSearch(this.state.strSearch)
	}

	// Change
	handleChange(event) {
		this.setState({
			strSearch : event.target.value
		})
	}

	// Clear form
	handleClear() {
		this.setState({
			strSearch : "",
		})
		this.props.onClickSearch("");
	}

	// Display
	render() {
		let elmButton = <button onClick={this.handleAdd} className="btn btn-primary btn-block">Add task</button>;
		if(this.props.isShowForm) {
			elmButton = <button onClick={this.handleAdd} className="btn btn-success btn-block">Close</button>
		}

    	return (
    		<>
            <div className="row mb-2">
                <div className="col-4">
					<div className="input-group btn-group">
                    	<input value={this.state.strSearch} type="Text" onChange={this.handleChange} className="col-8 form-control d-inline-flex" placeholder="Search..."></input>
						<button type="button" onClick={this.handleSearch} className="btn btn-info col-2">Go!</button>
						<button type="button" onClick={this.handleClear} className="btn btn-warning col-2">Clear</button>
					</div>
                </div>
			    <div className="col-2">
				    <div className="dropdown text-center">
					    <button className="btn btn-secondary dropdown-toggle" type="button" id="triggerId" data-toggle="dropdown" aria-haspopup="true"
					    		aria-expanded="false">
								Sort By
					    		</button>
				    	<div className="dropdown-menu" aria-labelledby="triggerId">
				    		<a className="dropdown-item" href="https://www.google.com/">Name ASC</a>
				    		<a className="dropdown-item" href="https://www.google.com/">Name DESC</a>
				    		<a className="dropdown-item" href="https://www.google.com/">Level ASC</a>
				    		<a className="dropdown-item" href="https://www.google.com/">Level DESC</a>
				    	</div>
				    </div>
				</div>
				<div className="col-6">
					{elmButton}
				</div>
			</div>
      		</>
    	);
  	}
}

export default ControlComponent;