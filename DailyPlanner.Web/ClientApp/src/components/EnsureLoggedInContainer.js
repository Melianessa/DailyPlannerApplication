import React, { Component } from "react";
import ReactDOM from 'react-dom';
import { BrowserRouter, Link, Route } from 'react-router-dom';

export class AddUser extends Component {
    componentDidMount() {
	    const { dispatch, currentUrl, redirectUrl } = this.props;
	    if (!window.token) {
		    // set the current url/path for future redirection (we use a Redux action)
		    // then redirect (we use a React Router method)
		    //dispatch(setRedirectUrl(currentUrl));
		    this.props.redirectUrl = currentUrl;
		    this.props.history.push("/account/login");
	    }
    }
    render() {
	    if (window.token) {
		    return this.props.children;
	    } else {
		    return null;
	    }
    }
}