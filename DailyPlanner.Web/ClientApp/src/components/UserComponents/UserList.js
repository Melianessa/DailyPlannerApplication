import React, { Component } from "react";
import { NavLink } from "reactstrap";
import { Link } from "react-router-dom";
import "../NavMenu.css";
import "../style.css";
import { confirmAlert } from "react-confirm-alert";
import "react-confirm-alert/src/react-confirm-alert.css";
import Pagination from "react-js-pagination";
import { NotificationContainer, NotificationManager } from "react-notifications";

export class UserList extends Component {
    static displayName = UserList.name;

    constructor(props) {
        super(props);
        this.state = { users: [], loading: true, activePage: 1, itemsPerPage: 3, status: "" };
        this.handleDelete = this.handleDelete.bind(this);
        this.helperDelete = this.helperDelete.bind(this);
        this.handlePageChange = this.handlePageChange.bind(this);
        this.renderUser = this.renderUser.bind(this);
        fetch("api/user/getAllUsers",
            {
                method: "GET",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json",
                    "Authorization": window.getToken()
                }
            })
            .then(response => {
                console.log(response);
                if (response.ok) {
                    return response.json();
                } else if (response.status === 401) {
                    this.setState({
                        status: response.statusText
                    });
                }
            }).then(data => {
                var users = data ? data : [];
                this.setState({
                    users: users, loading: false
                });
                console.log(this.props.location);
                if (this.props.location.state && this.props.location.state.actionMessage) {
                    NotificationManager.success("Success message", `User successfully ${this.props.location.state.actionMessage}!`, 3000, () => {
                        this.props.location.state.actionMessage = null;
                    });
                    console.log(this.props.location);
                }
            });
    }
    handlePageChange(pageNumber) {
        this.setState({ activePage: pageNumber });
    }

    helperDelete(id) {
        fetch("api/user/delete/" + id,
            {
                method: "DELETE",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json",
                    "Authorization": window.token
                }
            })
            .then(this.setState({
                users: this.state.users.filter((rec) => {
                    return (rec.id !== id);
                })
            }))
            .then(response => {
                console.log(response);
                if (response.ok) {
                    if (response.status === 200) {
                        NotificationManager.success("Success message", `User successfully deleted!`, 3000);
                    }
                    return response.json();
                } else if (response.status === 401) {
                    this.setState({
                        status: response.statusText
                    });
                }
            });
    }

    handleDelete(id) {
        confirmAlert({
            title: "Confirm to submit",
            message: "Do you want to delete this user?",
            buttons: [
                {
                    label: "Yes",
                    onClick: () => this.helperDelete(id)
                },
                {
                    label: "No",
                    onClick: () => { return; }
                }
            ]
        });
    }

    handleEdit(id) {
        this.props.match.params.id = id;
        this.props.history.push("/user/edit/" + id);
    }
    renderUser(users) {
	    
	    if (!users || users.length === 0) {
		    return <div>
                User list is empty
            </div>;
	    }
        return (
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th>Creation Date</th>
                        <th>Name</th>
                        <th>Date of birth</th>
                        <th>Phone</th>
                        <th>Email</th>
                        <th>Sex</th>
                        <th>Role</th>
                        <th>Event count</th>
                    </tr>
                </thead>
                <tbody>
                    {users.map((u, index) => {
                        var to = this.state.activePage * this.state.itemsPerPage;
                        var from = to - this.state.itemsPerPage;
                        if (index >= from && index < to) {
                            return <tr key={u.id}>
                                <td className="event-date-header">
                                    <div>{new Date(u.creationDate).toLocaleDateString()}</div>
                                </td>
                                <td>{u.firstName} {u.lastName}</td>
                                <td className="event-date-header">
                                    <div>{new Date(u.dateOfBirth).toLocaleDateString()}</div>
                                </td>
                                <td>{u.phone}</td>
                                <td>{u.email}</td>
                                <td>{u.sex ? "Male" : "Female"}</td>
                                <td>{u.role === 1 ? "Client" : "Admin"}</td>
                                <td>{u.eventCount}</td>
                                <td>
                                    <button className="btn btn-warning" onClick={() => this.handleEdit(u.id)}>Edit</button>
                                    <button className="btn btn-danger" onClick={() => this.handleDelete(u.id)}>Delete</button>
                                </td>
                            </tr>;
                        }
                    })}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderUser(this.state.users, this.state.itemsPerPage);
        if (this.state.status === "Unauthorized") {
	        return <div>
                <div>
	                You are {this.state.status.toLowerCase()}! Please <Link to="/account/login">login</Link> or <Link to="/account/register">register</Link> to continue :)
                </div>
            </div>;

        }
        return (
            <div>
                <h1>Users list</h1>
                <p>This component demonstrates users list.</p>
                <div className="button-group">
                    <NavLink tag={Link} className="btn btn-success" to="/user/create">Create new</NavLink>
                </div>
                {contents}
                <NotificationContainer />
                <Pagination
                    hideDisabled
                    activePage={this.state.activePage}
                    itemsCountPerPage={this.state.itemsPerPage}
                    totalItemsCount={this.state.users.length}
                    onChange={this.handlePageChange}
                />
            </div>
        );
    }
}