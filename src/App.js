import React, { Component } from 'react';
import './App.css';
import ListTaskComponent from './Components/ListTaskComponent';
import task from './Mocks/Task'
import ControlComponent from './Components/ControlComponent';
import Form from './Components/Form';
class App extends Component {
	constructor(props) {
		super(props);

		this.state = {
			items : task,
			isShowForm : false,
			strSearch : "",
			addItem : null,
		}
		this.handleToogleForm =	this.handleToogleForm.bind(this);
		this.closeForm = this.closeForm.bind(this);
		this.handleSearch = this.handleSearch.bind(this);
		this.handleAddItem = this.handleAddItem.bind(this);
	}

    handleToogleForm() {
        this.setState({
            isShowForm : !this.state.isShowForm,
        });
    }

	closeForm() {
		this.setState({
            isShowForm : false
        });
	}

	// Search
	handleSearch(value) {
		this.setState({
            strSearch : value,
        });
	}

	// Add item
	
	handleAddItem(value) {
		this.state.items.push([{
			id : "ok",
			name : value,
			level : 1,
		}]);
	}

	// Displayalue
	render() {
		let items = [];
		let search = this.state.strSearch.toLocaleLowerCase();

		if(search.length > 0) {
			this.state.items.forEach((item) => {
				if(item.name.toLocaleLowerCase().indexOf(search) !== -1)
				{
					items.push(item);
				}
			});
		}else{
			items = this.state.items;
		}

		let elmForm = null;
		if(this.state.isShowForm) {
			elmForm = <Form onlickSubmit={this.handleAddItem} onClickCancel={this.closeForm}/>;
		}

		return (
			<>
			<div className="mb-3">
				<h1 className="d-inline font-weight-normal">Project 01 - ToDo List</h1>
				<h3 className="d-inline font-weight-normal ml-5">Reacjs</h3>
			</div>
			<ControlComponent isShowForm={this.state.isShowForm}
				onClickAdd={this.handleToogleForm}
				onClickSearch={this.handleSearch}
			/>
			{elmForm}
			<ListTaskComponent ListItem={items}/>
			</>
		);
	}
}

export default App;
