import React from 'react';

export default () => {
    return(
        <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
        <div className="container">
            <a className="navbar-brand mr-auto" asp-action="Index" asp-controller="Home">Home</a>
            @if (User.IsInRole("admin"))
            {
                <a className="navbar-brand mx-auto" asp-action="Index" asp-controller="Admin">Добавить новости</a>
                }
            <label className="navbar-brand mr-4 mb-0 pt-1">@User.Identity.Name</label>
            <a className="navbar-brand" asp-action="Logout" asp-controller="Account">Выйти</a>
        </div>
    </nav>
    );
}