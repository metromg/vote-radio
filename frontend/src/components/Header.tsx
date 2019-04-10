import React from 'react';
import { Translate } from 'react-localize-redux';

const Header = () => {
    return (
        <div className="app-header-inner container">
            <div className="app-header-logo">
                <Translate id="appName" />
            </div>
        </div>
    );
}

export default Header;