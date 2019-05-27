import React, { Component } from "react";
import { confirmAlert } from "react-confirm-alert"; // Import
import "react-confirm-alert/src/react-confirm-alert.css";

export class Logout extends Component {
    static displayName = Logout.name;
    constructor(props) {
        super(props);
        this.state = {
            redirect: false
        }
        this.handleLogout = this.handleLogout.bind(this);
        this.helperLogout = this.helperLogout.bind(this);

        //TODO: implement storage or something where will be invalidate tokens or do something else
        //fetch("api/account/logout",
        //    {
        //        method: "GET",
        //        headers: {
        //            "Accept": "application/json",
        //            "Content-Type": "application/json",
        //            "Authorization": window.token
        //        }
        //    })
        //    .then(response => {
        //        console.log(response);
        //        if (response.ok) {
        //            localStorage.clear();
        //            window.token = "";
        //            this.props.history.push("/home");
        //        }
        //    });
    }
    helperLogout() {
        window.token = "";
        localStorage.clear();
        this.setState({ redirect: true });
    }
    handleLogout() {
        confirmAlert({
            title: "Confirm to submit",
            message: "Do you want to logout?",
            buttons: [
                {
                    label: "Yes",
                    onClick: () => this.helperLogout()
                },
                {
                    label: "No",
                    onClick: () => { return; }
                }
            ]
        });
    }
    renderRedirect() {
        if (this.state.redirect) {
            this.props.history.push("/");
        }
    }
    renderLogout() {
        return <div>
            <div className="form-group row">
                
                <div className="col-md-4">
                    <label className=" control-label col-md-12">You are successfully logout </label>
                </div>
            </div>
            {this.renderRedirect()}
        </div>;
    }
    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderLogout();
        return (
            <div>
                <h1>Logout</h1>
                {contents}
            </div>
        );
    }
}