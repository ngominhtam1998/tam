import React, { Component } from 'react';

class Item extends Component {
	constructor(props) {
		super(props);

		this.state = {
		
		}
	}

	// Display
  	render() {
		const item = this.props

    	return (
    		<>
			<tr>
				<th scope="row">{item.index + 1}</th>
				<td>{item.name}</td>
                <td>{this.showElementLevel(item.level)}</td>
                <td>
                    <button type="button" className="btn btn-warning">Edit</button>
                    <button type="button" className="btn btn-danger">Delete</button>
                </td>
			</tr>
      		</>
    	);
  	}
	
	showElementLevel(level) {
		let elmLevel = null;

		switch(level) {
			case 0:
				elmLevel = <span className="font-weight-bold text-secondary">Small</span>;
				  break;

			case 1:
				elmLevel = <span className="font-weight-bold text-primary">Medium</span>;
				  break;

			default:
				elmLevel = <span className="font-weight-bold text-danger">High</span>;
		}	
		return elmLevel;
	}
}

export default Item;