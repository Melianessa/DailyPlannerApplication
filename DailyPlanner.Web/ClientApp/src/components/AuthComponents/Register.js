﻿import React, { Component } from "react";
import "react-notifications/lib/notifications.css";
import "react-datepicker/dist/react-datepicker.css";
import "../NavMenu.css";
import "../style.css";
import "react-confirm-alert/src/react-confirm-alert.css";


export class Register extends Component {
    static displayName = Register.name;
    constructor(props) {
        super(props);
        this.state = {
            user: [],
            email: "",
            password: "",
            confirmPassword: "",
            loading: false
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
        let body = {
            Email: this.state.user.email,
            Password: this.state.user.password
        }
        fetch("account/register",
                {
                    method: "POST",
                    headers: {
                        "Accept": "application/json",
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(body)
                })//.then(NotificationManager.success('Success message', 'Event successfully added!', 3000))
            .then(this.setState({ redirect: true }));

    }
    renderRegisterForm(user) {
        return <div>
            <div className="form-group row">
                <label className=" control-label col-md-12">Login:</label>
                <div className="col-md-4">
                    <input className="form-control"
                        type="text"
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
            </div>
        );
    }
}