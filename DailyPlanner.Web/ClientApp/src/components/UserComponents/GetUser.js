import React, { Component } from "react";
import "react-notifications/lib/notifications.css";
import "react-datepicker/dist/react-datepicker.css";
import { Link } from "react-router-dom";
import male from "../img/male.png";
import female from "../img/female.png";


export class GetUser extends Component {
    constructor(props) {
        super(props);

        this.state = {
            user: [],
            loading: true,
            selectedRole: "",
            selectedSex: ""
        }
        this.startPage = this.startPage.bind(this);
        this.startPage();
    }
    startPage() {
        fetch("api/user/getUser",
            {
                method: "GET",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json",
                    "Authorization": window.token
                }
            }).then(response => {
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
    handleEdit(id) {
	    this.props.match.params.id = id;
	    this.props.history.push("/user/edit/" + id);
    }
    renderGetForm(user) {
        if (!user || user.length === 0) {
            return <div>
                User is empty
            </div>;
        }
        return <div className="container">
            <div className="row">
                <div className="col-lg-4">
                    <div className="form-group row">
                        <label className=" control-label col-md-12">Name:</label>
                        <div className="col-md-4">
                            {user.firstName} {user.lastName}
                        </div>
                    </div>
                    <div className="form-group row">
                        <label className=" control-label col-md-12">Date of birth: </label>
                        <div className="col-md-4">
                            {new Date(user.dateOfBirth).toLocaleDateString()}
                        </div>
                    </div>
                    <div className="form-group row">
                        <label className=" control-label col-md-12">Phone:</label>
                        <div className="col-md-4">
                            {user.phone}
                        </div>
                    </div>
                    <div className="form-group row">
                        <label className=" control-label col-md-12">Email:</label>
                        <div className="col-md-4">
                            {user.email}
                        </div>
                    </div>
                    <div className="form-group row">
                        <label className=" control-label col-md-12">Sex: </label>
                        <div className="col-md-4">
                            {user.sex ? "Male" : "Female"}
                        </div>
                    </div >
                    <div className="form-group row">
                        <label className=" control-label col-md-12">Role: </label>
                        <div className="col-md-4">
                            {user.role === 1 ? "Client" : "Admin"}
                        </div>
                    </div >
                </div>
                <div className="col-lg-4">
                    <div className="form-group row">
                        <img src={user.sex ? male : female} width="200" height="200" alt="User picture"/>
                    </div>
	                                </div>
	            <div className="col-lg-4">
		            <div className="form-group row">
			            <button className="btn btn-warning" onClick={() => this.handleEdit(this.state.user.id)}>Edit information</button>
		            </div>
	            </div>
            </div>
        </div>;
    }
    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderGetForm(this.state.user);
        if (this.state.status === "Unauthorized") {
            return <div>
                <div>
                    You are {this.state.status.toLowerCase()}! Please <Link to="/account/login">login</Link> or <Link to="/account/register">register</Link> to continue :)
                </div>
            </div>;
        }
        return (
            <div>
                <h1>User information</h1>
                {contents}
            </div>
        );
    }
}