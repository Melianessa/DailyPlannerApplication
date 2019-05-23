import React, { Component } from "react";
import { confirmAlert } from "react-confirm-alert"; // Import
import "react-confirm-alert/src/react-confirm-alert.css";

export class Logout extends Component {
    static displayName = Logout.name;
    constructor(props) {
        super(props);
        this.handleLogout = this.handleLogout.bind(this);
        this.helperLogout = this.helperLogout.bind(this);

        fetch("api/account/logout",
	        {
		        method: "GET",
		        headers: {
			        "Accept": "application/json",
			        "Content-Type": "application/json",
			        "Authorization": window.token
		        }
	        });
    }
    helperLogout() {
        window.token = "";
        this.props.history.push("/");
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
}