﻿import { Component } from "@angular/core";

@Component({
    selector: "app-home",
    template: require("./home.component.html")
})
export class HomeComponent {

    public selection: Array<any> = [];
}