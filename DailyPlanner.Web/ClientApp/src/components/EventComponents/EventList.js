import React, { Component } from "react";
import { NavLink } from "reactstrap";
import { Link } from "react-router-dom";
import "../NavMenu.css";
import "../style.css";
import { confirmAlert } from "react-confirm-alert"; // Import
import "react-confirm-alert/src/react-confirm-alert.css";
import "react-datepicker/dist/react-datepicker.css";
import DatePicker from "react-datepicker";
import { NotificationContainer, NotificationManager } from "react-notifications";


export class EventList extends Component {
    static displayName = EventList.name;

    constructor(props) {
        super(props);
        let date = new Date();
        this.state = { events: [], loading: true, selectedDay: date, offset: new Date().getTimezoneOffset(), status: "" };
        this.handleDelete = this.handleDelete.bind(this);
        this.helperDelete = this.helperDelete.bind(this);
        this.handleDayClick = this.handleDayClick.bind(this);
        this.handleGetAll = this.handleGetAll.bind(this);
        this.handleGetAll(new Date());
    }
    handleGetAll(day) {
        let reqBody = { date: day.toLocaleDateString("en-US") };
        fetch("api/event/getByDate", {
            method: "POST",
            headers: {
                //"Accept": "application/json",
                //application/x-www-form-urlencoded for methods without [FromBody]
                //application/json for methods with [FromBody]
                "Content-Type": "application/x-www-form-urlencoded",
                "Authorization": window.token
            },
            body: new URLSearchParams(reqBody).toString()
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
            var events = data ? data : [];
            this.handleDayClick(day);
            for (var i = 0; i < events.length; i++) {
                let startMinutes = new Date(events[i].startDate).setMinutes(new Date(events[i].startDate).getMinutes() -
                    this.state.offset);
                let endMinutes = new Date(events[i].endDate).setMinutes(new Date(events[i].endDate).getMinutes() -
                    this.state.offset);
                events[i].startDate = new Date(startMinutes).toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" });
                events[i].endDate = new Date(endMinutes).toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" });
            }
            this.setState({
                events: events, loading: false
            });
            if (this.props.location.state && this.props.location.state.actionMessage) {
                NotificationManager.success("Success message", `Event successfully ${this.props.location.state.actionMessage}!`, 3000);
                this.props.location.state.actionMessage = null;
                console.log(this.props.location);
            }
        });

    }

    handleDayClick(day) {
        this.setState({ selectedDay: day });
    }
    helperDelete(id) {
        fetch("api/event/delete/" + id,
            {
                method: "DELETE",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json",
                    "Authorization": window.token
                }
            })
            .then(this.setState({
                events: this.state.events.filter((rec) => {
                    return (rec.id !== id);
                })
            }))
            .then(response => {
                console.log(response);
                if (response.ok) {
                    if (response.status === 200) {
                        NotificationManager.success("Success message", `Event successfully deleted!`, 3000);
                    }
                    return response.json();
                } else if (response.status === 401) {
                    this.setState({
                        status: response.statusText
                    });
                }
            });
    }

    handleDelete(id) {
        confirmAlert({
            title: "Confirm to submit",
            message: "Do you want to delete these event?",
            buttons: [
                {
                    label: "Yes",
                    onClick: () => this.helperDelete(id)
                },
                {
                    label: "No",
                    onClick: () => { return; }
                }
            ]
        });
    }

    handleEdit(id) {
        this.props.match.params.id = id;
        this.props.history.push("/event/edit/" + id);

    }
    renderEvent(events) {
        if (!events || events.length === 0) {
            return <table className="table table-striped">
                <thead>
                    <tr>
                        <th>Date: <DatePicker onChange={this.handleGetAll} selected={this.state.selectedDay} /></th>
                        <th>Event</th>
                    </tr>
                </thead>
                <div>
                    There is no events to this date
                </div>
            </table>;
        }
        return (
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th>Date: <DatePicker onChange={this.handleGetAll} selected={this.state.selectedDay} /></th>
                        <th>Event</th>
                    </tr>
                </thead>
                <tbody>
                    {events.map(ev =>
                        <tr key={ev.id}>
                            <td className="event-date-header">
                                <div>{ev.startDate}</div>
                                <div>{ev.endDate}</div>
                            </td>
                            <td className="event-date-header">
                                <div>{ev.title}</div>
                                <div>{ev.description}</div>

                            </td>
                            <td>
                                <button className="btn btn-warning" onClick={() => this.handleEdit(ev.id)}>Edit</button>
                                <button className="btn btn-danger" onClick={() => this.handleDelete(ev.id)}>Delete</button>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderEvent(this.state.events);
        if (this.state.status === "Unauthorized") {
            return <div>
                <div>
                    You are {this.state.status.toLowerCase()}! Please <Link to="/account/login">login</Link> or <Link to="/account/register">register</Link> to continue :)
                </div>
            </div>;
        }
        if (this.state.status === "Forbidden") {
            return <div>
                <div>
                    You haven't access to this page :)
                </div>
            </div>;
        }
        return (
            <div>
                <h1>Events list</h1>
                <p>This component demonstrates events list.</p>
                <div className="button-group">
                    <NavLink tag={Link} className="btn btn-success" to="/event/create">Create new</NavLink>
                </div>
                {contents}
                <NotificationContainer />
            </div>
        );
    }
}