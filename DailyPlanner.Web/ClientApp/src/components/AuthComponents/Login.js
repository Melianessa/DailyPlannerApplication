import React, { Component } from "react";
import "react-notifications/lib/notifications.css";
import "react-datepicker/dist/react-datepicker.css";
import "../NavMenu.css";
import "../style.css";
import "react-confirm-alert/src/react-confirm-alert.css";
import { NavMenu } from "../NavMenu"


export class Login extends Component {
    static displayName = Login.name;
    constructor(props) {
        super(props);
        
        this.state = {
            user: [],
            login: "",
            password: "",
            loading: false,
            isSuccess: false,
            redirect: false
        }
        this.handleClick = this.handleClick.bind(this);
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
            Username: this.state.user.login,
            Password: this.state.user.password
        }
        fetch("api/account/login",
            {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(body)
            }).then(response => {
                const json = response.json();
                return json;
            }).then(data => {
                if (data.token) {
                    let localStorageValues = [data.token, data.expirationTime, data.refreshToken];
                    let localStorageKeys = ["token", "expirationTime", "refreshToken"];
                    localStorage.setItem("tokenData", JSON.stringify(localStorageValues));
                    window.token = `Bearer ${data.token}`;
                }
                this.setState({
                    isSuccess: data.isSuccess
                });
                if (this.state.isSuccess) {
                    this.setState({ redirect: true });
                }
            });
    }
    renderRedirect() {
        if (this.state.redirect) {
            this.props.history.push("/user/list");
        }
    }
    renderLoginForm(user) {
        return <div>
            <div className="form-group row">
                <label className=" control-label col-md-12">Login:</label>
                <div className="col-md-4">
                    <input className="form-control"
                        type="email"
                        value={user.login}
                        onChange={this.handleChange.bind(this, "login")} />
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
            <div className="form-group">
                <button className="btn btn-success" onClick={this.handleClick}>Login</button>
                <button className="btn btn-danger" onClick={this.handleCancel.bind(this)}>Cancel</button>
            </div>
            {this.renderRedirect()}
        </div>;
    }
    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderLoginForm(this.state.user);
        return (
            <div>
                <h1>Login</h1>
                {contents}
            </div>
        );
    }
}