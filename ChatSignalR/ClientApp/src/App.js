import React from 'react';
import {BrowserRouter, Route,Switch} from "react-router-dom";
import {Login} from "./components/Login";
import {Chat} from "./components/Chat"
import { ChatState } from './components/Chat/ChatState';
import {PrivateRoute} from "./components/PrivateRoute";
import {Register} from "./components/Register";

export const App=()=>{
  return(
    <BrowserRouter>
            <Switch>
                <Route path="/login" component={Login} />
                <Route path="/register" component={Register}></Route>
                <PrivateRoute path="/chatReact" component={SetState} exact />            
            </Switch>
      </BrowserRouter>
  )
}

const SetState=()=>{
  return (
    <ChatState>
      <Chat />  
    </ChatState> 
  )
}