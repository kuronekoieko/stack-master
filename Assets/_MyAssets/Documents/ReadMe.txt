ステージの作り方

■概要
・ステージはまるごとプレハブにする
・ゲームシーンは一つで、再生時にステージのプレハブを生成する
・次のステージに移るときは、シーンをリロードする

■ステージのプレハブ
・Assets/_MyAssets/Prefabs/Game/Stagesに何個かあるので、真似して作る
・ルートのオブジェクトは座標(0,0,0)にする（preferences→SceneView→Create Object at Originにチェックをつけると良い）
・プレハブ名は「自分の名前の頭文字+3桁の番号」にする（例:「i001」）
・ステージで使うパーツのプレハブはAssets/_MyAssets/Prefabs/Game/StagePartsに入れる
・動作確認はAssets/_MyAssets/Scenes/Stages/Stage.unityに置いて実行するだけ

※注意
Assets/_MyAssets/Scenes/Stages/Stage.unityは変更してコミットしない
もしコミットした場合、できる限り変更前に戻す
masterからプルして競合が発生したら「相手の変更で解決」をする
