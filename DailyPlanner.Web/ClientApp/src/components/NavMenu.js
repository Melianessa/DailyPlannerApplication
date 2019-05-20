import React, { Component } from "react";
import { Link } from "react-router-dom";
import { Glyphicon, Nav, Navbar, NavItem } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";
import "./NavMenu.css";

export class NavMenu extends Component {
    displayName = NavMenu.name

    renderLoginData() {
        if (true) {
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
			                    <Glyphicon glyph='user' /> User
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
                    </Nav>
                </Navbar.Collapse>
            </Navbar>
        );
    }
}
