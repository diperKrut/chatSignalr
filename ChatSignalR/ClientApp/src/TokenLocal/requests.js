import Axios from "axios"
import * as signalR from "@aspnet/signalr";
import {TokenLocal} from "./TokenLocal";

export class InitiliazeData{
    static async getData(){
        let response= await Axios.get("https://localhost:44302/chat/initiliazation",{headers:{"Authorization": "Bearer "+ TokenLocal.getToken()}});
        return response.data;
    }
    static getChatHub(){
        return new signalR.HubConnectionBuilder()
        .withUrl("/chat",{ accessTokenFactory: () => TokenLocal.getToken()})
        .build();
    }
}
