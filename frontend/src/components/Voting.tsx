import React, { Component } from 'react';
import { connect } from 'react-redux';

import { AppState } from '../store';
import { VotingCandidate } from '../store/voting/types';
import { loadVotingCandidates } from '../store/voting/actions';

interface VotingProps {
    candidates: VotingCandidate[];

    loadVotingCandidates: () => void;
}

class Voting extends Component<VotingProps> {
    componentDidMount() {
        this.props.loadVotingCandidates();
    }

    render() {
        return (
            <ul>
                {this.props.candidates.map(candidate => 
                    <li key={candidate.songId}>{candidate.title} {candidate.voteCount}</li>
                )}
            </ul>
        );
    }
}

const mapStateToProps = (state: AppState) => ({
    candidates: state.voting.candidates
});

const mapDispatchToProps = (dispatch: any) => ({
    loadVotingCandidates: () => dispatch(loadVotingCandidates())
});

export default connect(mapStateToProps, mapDispatchToProps)(Voting);