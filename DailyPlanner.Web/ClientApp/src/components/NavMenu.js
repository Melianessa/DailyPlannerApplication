import React, { Component } from "react";
import { Link } from "react-router-dom";
import { Glyphicon, Nav, Navbar, NavItem } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";
import "./NavMenu.css";
import { confirmAlert } from "react-confirm-alert"; // Import
import "react-confirm-alert/src/react-confirm-alert.css";

export class NavMenu extends Component {
    displayName = NavMenu.name
    constructor(props) {
        super(props);
        this.state = { render: "" }
        this.setState({ render: window.token });
        this.helperLogout = this.helperLogout.bind(this);
    }
    helperLogout() {
	    window.token = "";
	    localStorage.clear();
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
    renderLoginData() {
        if (this.state.render) {
            return <Nav>
                <LinkContainer to={"/user/list"}>
                    <NavItem>
                        <Glyphicon glyph='user' /> User
				           </NavItem>
                </LinkContainer>
                <LinkContainer to={"/event/list"}>
                    <NavItem>
                        <Glyphicon glyph='list-alt' /> Events
				           </NavItem>
                </LinkContainer>
            </Nav>;
        }
    }
    render() {
        return (
            <Navbar inverse fixedTop fluid collapseOnSelect>
                <Navbar.Header>
                    <Navbar.Brand>
                        <Link to={"/"}>Daily Planner</Link>
                    </Navbar.Brand>
                    <NavItem>
                        <Link to={"/account/login"}>Login</Link>
                        <Link to={"/account/register"}>Register</Link>
                        <Link to={"/logout"} onClick={() => this.handleLogout()}>Logout</Link>
                    </NavItem>
                    <Navbar.Toggle />
                </Navbar.Header>
                <Navbar.Collapse>
                    <Nav>
                        <LinkContainer to={"/"} exact>
                            <NavItem>
                                <Glyphicon glyph='home' /> Home
              </NavItem>
                        </LinkContainer>
                        <LinkContainer to={"/user/list"}>
                            <NavItem>
                                <Glyphicon glyph='user' /> Users
		                    </NavItem>
                        </LinkContainer>
                        <LinkContainer to={"/event/list"}>
                            <NavItem>
                                <Glyphicon glyph='list-alt' /> Events
		                    </NavItem>
                        </LinkContainer>
                        <LinkContainer to={"/swagger"}>
                            <NavItem>
                                <Glyphicon glyph='eye-open' /> Swagger API
		                    </NavItem>
                        </LinkContainer>
                        <LinkContainer to={"/user/info"}>
                            <NavItem>
                                <Glyphicon glyph='user' /> My info
		                    </NavItem>
                        </LinkContainer>
                    </Nav>
                </Navbar.Collapse>
            </Navbar>
        );
    }
}
