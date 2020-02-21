import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import {TokenLocal} from "../TokenLocal/TokenLocal"

export const PrivateRoute = ({component: Component, ...rest}) => {
    return (

       
        <Route {...rest} render={props => (
            TokenLocal.getIsAuth() ?
                <Component {...props} />
            : <Redirect to={"/login"} />
        )} />
    );
};