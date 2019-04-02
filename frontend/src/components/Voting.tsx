import React, { Component } from 'react';
import { connect } from 'react-redux';

import { AppState } from '../store';
import { VotingCandidate } from '../store/voting/types';
import { loadVotingCandidates, selectVotingCandidate } from '../store/voting/actions';

interface VotingProps {
    candidates: VotingCandidate[];
    selectedSongId: string | null;

    loadVotingCandidates: () => void;
    selectVotingCandidate: (songId: string) => void;
}

class Voting extends Component<VotingProps> {
    componentDidMount() {
        this.props.loadVotingCandidates();
    }

    render() {
        return (
            <ul>
                {this.props.candidates.map(candidate => {
                    const isSelected = candidate.songId === this.props.selectedSongId;
                    const style = isSelected
                        ? { background: "red" }
                        : undefined;

                    return (
                        <li key={candidate.songId} style={style}>
                            {candidate.title} {candidate.voteCount}
                            {
                                this.props.selectedSongId == null
                                    ? <button onClick={() => this.props.selectVotingCandidate(candidate.songId)}>Vote</button>
                                    : null
                            }
                        </li>
                    );
                })}
            </ul>
        );
    }
}

const mapStateToProps = (state: AppState) => ({
    candidates: state.voting.candidates,
    selectedSongId: state.voting.selectedSongId
});

const mapDispatchToProps = (dispatch: any) => ({
    loadVotingCandidates: () => dispatch(loadVotingCandidates()),
    selectVotingCandidate: (songId: string) => dispatch(selectVotingCandidate(songId))
});

export default connect(mapStateToProps, mapDispatchToProps)(Voting);