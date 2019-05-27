import React, { Component } from "react";
import "react-notifications/lib/notifications.css";
import "react-datepicker/dist/react-datepicker.css";
import DatePicker from "react-datepicker";
import "../NavMenu.css";
import "../style.css";
import { staticData } from "../Context";
import "react-confirm-alert/src/react-confirm-alert.css";
import { Link } from "react-router-dom";

export class EditUser extends Component {
    static displayName = EditUser.name;
    constructor(props) {
        super(props);
        let roleList = staticData.roleList;
        let sexList = staticData.sexList;
        this.state = {
            user: [],
            firstName: "",
            lastName: "",
            dateOfBirth: new Date(),
            phone: "",
            email: "",
            sex: sexList,
            role: roleList,
            selectedRole: null,
            selectedSex: null,
            redirect: false,
            loading: true,
            status: ""
        }

        this.handleClick = this.handleClick.bind(this);
        this.startPage = this.startPage.bind(this);
        this.startPage(this.props.match.params.id);
    }
    startPage(id) {
        fetch("api/user/edit/" + id,
            {
                method: "GET",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json",
                    "Authorization": window.token
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
                var user = data ? data : [];
                this.setState({
                    user: user, loading: false, selectedRole: user.role, selectedSex: user.sex
                });
            });
    }
    handleChange(propertyName, event) {
        const user = this.state.user;
        if (propertyName === "dateOfBirth") {
            user[propertyName] = event;
        } else {
            user[propertyName] = event.target.value;
        }
        if (propertyName === "sex") {
            this.setState({ selectedSex: event.target.value });
        }
        if (propertyName === "role") {
            this.setState({ selectedRole: event.target.value });
        }
        this.setState({ user: user });
    }
    handleClick(id) {
        let body = {
            FirstName: this.state.user.firstName,
            LastName: this.state.user.lastName,
            DateOfBirth: this.state.user.dateOfBirth,
            Phone: this.state.user.phone,
            Email: this.state.user.email,
            Sex: this.state.selectedSex,
            Role: this.state.selectedRole,
            Id: this.state.user.id
        }
        fetch("api/user/edit/" + id,
            {
                method: "PUT",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json",
                    "Authorization": window.token
                },
                body: JSON.stringify(body)
            }).then(response => {
                console.log(response);
                if (response.ok) {
                    return response.json();
                } else if (response.status === 401) {
                    this.setState({
                        status: response.statusText
                    });
                }
            })
            .then(data => {
                var user = data ? data : [];
                this.setState({ user: user, redirect: true });
            });
    }
    handleCancel() {
        this.props.history.push("/user/list");
    }
    renderRedirect() {
        if (this.state.redirect) {
            this.props.history.push({
                pathname: "/user/list",
                state: { actionMessage: "edited" }
            });
        }
    }
    renderEditForm(user) {
        if (!user || user.length === 0) {
            return <div>
                User is empty
            </div>;
        }
        return <div>
            <div className="form-group row">
                <label className=" control-label col-md-12">First Name:</label>
                <div className="col-md-4">
                    <input className="form-control"
                        type="text"
                        value={user.firstName}
                        onChange={this.handleChange.bind(this, "firstName")} />
                </div>
            </div>
            <div className="form-group row">
                <label className=" control-label col-md-12">Last Name:</label>
                <div className="col-md-4">
                    <input className="form-control"
                        type="text"
                        value={user.lastName}
                        onChange={this.handleChange.bind(this, "lastName")} />
                </div>
            </div>
            <div className="form-group row">
                <label className=" control-label col-md-12">Date of birth: </label>
                <div className="col-md-4">
                    <DatePicker className="form-control"
                        selected={user.dateOfBirth}
                        onChange={this.handleChange.bind(this, "dateOfBirth")}
                        showYearDropdown
                        scrollableYearDropdown
                        yearDropdownItemNumber={15}
                        showMonthDropdown
                    />
                </div>
            </div>
            <div className="form-group row">
                <label className=" control-label col-md-12">Phone:</label>
                <div className="col-md-4">
                    <input className="form-control"
                        type="phone"
                        value={user.phone}
                        onChange={this.handleChange.bind(this, "phone")} />
                </div>
            </div>
            <div className="form-group row">
                <label className=" control-label col-md-12">Email:</label>
                <div className="col-md-4">
                    <input className="form-control"
                        type="email"
                        value={user.email}
                        onChange={this.handleChange.bind(this, "email")} />
                </div>
            </div>
            <div className="form-group row">
                <label className=" control-label col-md-12">Sex: </label>
                <div className="col-md-4">
                    <select className="form-control" value={this.state.selectedSex} onChange={this
                        .handleChange.bind(this, "sex")} >
                        <option value="">-- Select sex --</option>
                        {this.state.sex.map(p =>
                            <option key={p.name} value={p.value}>{p.name}</option>
                        )}
                    </select>
                </div>
            </div >
            <div className="form-group row">
                <label className=" control-label col-md-12">Role: </label>
                <div className="col-md-4">
                    <select className="form-control" value={this.state.selectedRole} onChange={this
                        .handleChange.bind(this, "role")} >
                        <option value="">-- Select role --</option>
                        {this.state.role.map(p =>
                            <option key={p.name} value={p.value}>{p.name}</option>
                        )}
                    </select>
                </div>
            </div >

            <div className="form-group">
                <button className="btn btn-success" onClick={this.handleClick}>Save user</button>
                <button className="btn btn-danger" onClick={this.handleCancel.bind(this)}>Cancel</button>
            </div>
            {this.renderRedirect()}
        </div>;
    }
    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderEditForm(this.state.user);
        if (this.state.status === "Unauthorized") {
            return <div>
                <div>
                    You are {this.state.status.toLowerCase()}! Please <Link to="/account/login">login</Link> or <Link to="/account/register">register</Link> to continue :)
                </div>
            </div>;
        }
        return (
            <div>
                <h1>Edit user</h1>
                <p>Edit the following fields.</p>

                {contents}
            </div>
        );
    }
}