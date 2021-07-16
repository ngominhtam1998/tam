import React, { Component } from 'react';
import Item from './Item';

class ListTaskComponent extends Component {
	constructor(props) {
		super(props);

		this.state = {
		
		}
	}
	// Display
  	render() {
		const items = this.props.ListItem;

		// Foreach
		const elmItem = items.map((item, index) => {
			return (
				<Item key={index} index={index} name={item.name} level={item.level} />
			);
		})
    	return (
    		<>
			<div className="table-responsive">
				<table className="table table-striped">
			  		<thead>
						<tr>
				  			<th scope="col">#</th>
				 			<th className="w-50" scope="col">Task</th>
				  			<th style={{width: "15%"}} scope="col">Level</th>
				  			<th scope="col">Action</th>
						</tr>
			  		</thead>
			  		<tbody>
					  	{elmItem}
					</tbody>
				</table>
			</div>
      		</>
    	);
  	}
}

export default ListTaskComponent;