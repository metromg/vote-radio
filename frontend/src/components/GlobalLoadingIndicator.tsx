import React, { ReactNode } from 'react';

import LoadingSpinner from './LoadingSpinner';
import './GlobalLoadingIndicator.css';

interface GlobalLoadingIndicatorProps {
    loading: boolean;
    children: ReactNode;
}

export const GlobalLoadingIndicator = (props: GlobalLoadingIndicatorProps) => {
    if (props.loading) {
        return (
            <div className="loading-indicator">
                <LoadingSpinner />
            </div>
        );
    }

    return <React.Fragment>{props.children}</React.Fragment>;
}

export default GlobalLoadingIndicator;