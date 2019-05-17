import React, { Component } from "react";
import "react-notifications/lib/notifications.css";
import "react-datepicker/dist/react-datepicker.css";
import "../NavMenu.css";
import "../style.css";
import "react-confirm-alert/src/react-confirm-alert.css";
import { NotificationContainer, NotificationManager } from "react-notifications";


export class Register extends Component {
    static displayName = Register.name;
    constructor(props) {
        super(props);
        this.state = {
            user: [],
            email: "",
            password: "",
            firstName: "",
            lastName: "",
            confirmPassword: "",
            loading: false,
            isSuccess: false,
            redirect: false,
            isValid: true
        }
        this.handleClick = this.handleClick.bind(this);
        this.handleCancel = this.handleCancel.bind(this);
    }
    handleChange(propertyName, event) {
        const user = this.state.user;
        user[propertyName] = event.target.value;
        this.setState({ user: user });
    }
    handleCancel() {
        return;
    }
    handleClick() {
        const { password, confirmPassword, email} = this.state.user;
        if (!email.includes("@")||!email.includes(".")) {
	        NotificationManager.error("Error message",
		        `Email is invalid`,
		        2000);
        }
        if (password !== confirmPassword) {
	        this.setState({ isValid: false });
            NotificationManager.error("Error message",
                `Passwords don't match`,
                2000);
        } else if (password.length < 6) {
            NotificationManager.error("Error message",
                `Password should be a minimun of six (6) characters in length`, 2500);
        } else {
            let body = {
                Username: this.state.user.email,
                Password: this.state.user.password,
                FirstName: this.state.user.firstName,
                LastName: this.state.user.lastName
            }
            fetch("api/account/register",
                {
                    method: "POST",
                    headers: {
                        "Accept": "application/json",
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(body)
                }).then(response => {
                    const json = response.json();
                    console.log(json);
                    return json;
                }).then(data => {
                    console.log(data);
                    console.log(this.state.user);
                    this.setState({
                        isSuccess: data.isSuccess
                    });
                    if (this.state.isSuccess) {
                        this.setState({ redirect: true });
                    }
                });
        }
    }
    renderRedirect() {
        if (this.state.redirect) {
            this.props.history.push("/user/list");
        }
    }
    renderRegisterForm(user) {
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
                <label className=" control-label col-md-12">Email:</label>
                <div className="col-md-4">
                    <input className="form-control"
                        type="email"
                        value={user.email}
                        onChange={this.handleChange.bind(this, "email")} />
                </div>
            </div>
            <div className="form-group row">
                <label className=" control-label col-md-12">Password:</label>
                <div className="col-md-4">
                    <input className="form-control"
                        type="password"
                        value={user.password}
                        onChange={this.handleChange.bind(this, "password")} />
                </div>
            </div>
            <div className="form-group row">
                <label className=" control-label col-md-12">Confirm password:</label>

                <div className="col-md-4">
                    <input className="form-control"
                        type="password"
                        value={user.confirmPassword}
                        onChange={this.handleChange.bind(this, "confirmPassword")} />
                </div>
            </div>
            <div className="form-group">
                <button className="btn btn-success" onClick={this.handleClick}>Save</button>
                <button className="btn btn-danger" onClick={this.handleCancel.bind(this)}>Cancel</button>
            </div>
            {this.renderRedirect()}
        </div>;
    }
    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderRegisterForm(this.state.user);
        return (
            <div>
                <h1>Registration form</h1>
                {contents}
                <NotificationContainer />
            </div>
        );
    }
}