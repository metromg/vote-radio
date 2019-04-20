import React, { Component } from 'react';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';
import { Translate } from 'react-localize-redux';

import { AppState } from '../store';
import { dismissError } from '../store/error/actions';
import './GlobalError.css';

interface GlobalErrorProps {
    errorMessageKey: string | null;

    dismiss: () => void;
}

export const GlobalError = (props: GlobalErrorProps) => {
    if (props.errorMessageKey == null) {
        return null;
    }

    return (
        <div className="error-message">
            <span>
                <Translate id={props.errorMessageKey} />
            </span>
            <button type="button" onClick={() => props.dismiss()}>×</button>
        </div>
    );
}

const mapStateToProps = (state: AppState) => ({
    errorMessageKey: state.error.errorMessageKey
});

const mapDispatchToProps = (dispatch: Dispatch) => ({
    dismiss: () => dispatch(dismissError())
});

export default connect(mapStateToProps, mapDispatchToProps)(GlobalError);