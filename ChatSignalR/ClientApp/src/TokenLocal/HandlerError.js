import axios from "axios";
import {TokenLocal} from "./TokenLocal";
import React from "react";
import {Redirect} from "react-router-dom";

export const HandlerExpiredError= async (err, callback)=>{
    if(err.response.status===401&&err.response.headers["token-expired"]==="true"){
        let res= await axios.post("https://localhost:44302/test/token", {refreshToken: TokenLocal.getRefreshToken(),
            accesToken: TokenLocal.getToken()
        })
        TokenLocal.SetRefreshToken(res.data.refresh);
        TokenLocal.SetToken(res.data.jwt);
        return callback();
    }
    console.log("INVALID TOKEN")
   
}