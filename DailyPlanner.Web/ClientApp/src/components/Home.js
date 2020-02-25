import React, { Component } from "react";
import { Link } from "react-router-dom";

export class Home extends Component {
    displayName = Home.name

    render() {
        return (
            <div>
                <h1>Hello, user!</h1>
                <p>Welcome to your daily planner, where you can:</p>
                <ul>
                    <li><strong>Plan your business day</strong></li>
                    <li><strong>Plan your weekends</strong></li>
                    <li><strong>Create and Manage Events very easy</strong></li>
                </ul>
                <p> Please <Link to="/account/login">login</Link> or <Link to="/account/register">register</Link> to see your events :)</p>
            </div>
        );
    }
}
