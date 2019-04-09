import React, { ReactNode } from 'react';

import './GlobalLoadingIndicator.css';

interface GlobalLoadingIndicatorProps {
    loading: boolean;
    children: ReactNode;
}

const GlobalLoadingIndicator = (props: GlobalLoadingIndicatorProps) => {
    if (props.loading) {
        return (
            <div className="loading-indicator">
                <div className="lds-ripple">
                    <div></div>
                    <div></div>
                </div>
            </div>
        );
    }

    return <React.Fragment>{props.children}</React.Fragment>;
}

export default GlobalLoadingIndicator;